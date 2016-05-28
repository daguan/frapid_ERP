using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Frapid.NPoco
{
    public class PocoDataBuilder
    {
        private readonly Cache<string, Type> _aliasToType = Cache<string, Type>.CreateStaticCache();

        protected Type Type { get; set; }
        private MapperCollection Mapper { get; set; }

        private List<PocoMemberPlan> _memberPlans { get; set; }
        private TableInfoPlan _tableInfoPlan { get; set; }

        private delegate PocoMember PocoMemberPlan(TableInfo tableInfo);
        protected delegate TableInfo TableInfoPlan();

        public PocoDataBuilder(Type type, MapperCollection mapper)
        {
            this.Type = type;
            this.Mapper = mapper;
        }

        public PocoDataBuilder Init()
        {
            List<MemberInfo> memberInfos = new List<MemberInfo>();
            ColumnInfo[] columnInfos = this.GetColumnInfos(this.Type);

            // Get table info plan
            this._tableInfoPlan = this.GetTableInfo(this.Type, columnInfos, memberInfos);

            // Get pocomember plan
            this._memberPlans = this.GetPocoMembers(this.Mapper, columnInfos, memberInfos).ToList();

            return this;
        }

        private ColumnInfo[] GetColumnInfos(Type type)
        {
            return ReflectionUtils.GetFieldsAndPropertiesForClasses(type)
                .Where(x => !IsDictionaryType(x.DeclaringType))
                .Select(x => this.GetColumnInfo(x, type)).ToArray();
        }

        public static bool IsDictionaryType(Type type)
        {
            return new[] { typeof(object), typeof(IDictionary<string, object>), typeof(Dictionary<string, object>) }.Contains(type);
        }

        public TableInfo BuildTableInfo()
        {
            return this._tableInfoPlan();
        }

        public PocoData Build()
        {
            PocoData pocoData = new PocoData(this.Type, this.Mapper);

            pocoData.TableInfo = this._tableInfoPlan();

            pocoData.Members = this._memberPlans.Select(plan => plan(pocoData.TableInfo)).ToList();

            pocoData.Columns = GetPocoColumns(pocoData.Members, false).Where(x => x != null).ToDictionary(x => x.ColumnName, x => x, StringComparer.OrdinalIgnoreCase);
            pocoData.AllColumns = GetPocoColumns(pocoData.Members, true).Where(x => x != null).ToList();

            //Build column list for automatic select
            pocoData.QueryColumns = pocoData.Columns.Where(c => !c.Value.ResultColumn && c.Value.ReferenceType == ReferenceType.None).ToArray();

            return pocoData;
        }

        protected virtual TableInfoPlan GetTableInfo(Type type, ColumnInfo[] columnInfos, List<MemberInfo> memberInfos)
        {
            string alias = this.CreateAlias(type.Name, type);
            TableInfo tableInfo = TableInfo.FromPoco(type);
            tableInfo.AutoAlias = alias;
            return () => { return tableInfo.Clone(); };
        }

        protected virtual ColumnInfo GetColumnInfo(MemberInfo mi, Type type)
        {
            return ColumnInfo.FromMemberInfo(mi);
        }

        private static IEnumerable<PocoColumn> GetPocoColumns(IEnumerable<PocoMember> members, bool all)
        {
            foreach (PocoMember member in members)
            {
                if (all || (member.ReferenceType != ReferenceType.OneToOne
                            && member.ReferenceType != ReferenceType.Many))
                {
                    yield return member.PocoColumn;
                }

                if (all || (member.ReferenceType == ReferenceType.None))
                {
                    foreach (PocoColumn pocoMemberChild in GetPocoColumns(member.PocoMemberChildren, all))
                    {
                        yield return pocoMemberChild;
                    }
                }
            }
        }

        private IEnumerable<PocoMemberPlan> GetPocoMembers(MapperCollection mapper, ColumnInfo[] columnInfos, List<MemberInfo> memberInfos, string prefix = null)
        {
            MemberInfo[] capturedMembers = memberInfos.ToArray();
            string capturedPrefix = prefix;
            foreach (ColumnInfo columnInfo in columnInfos)
            {
                if (columnInfo.IgnoreColumn)
                    continue;

                Type memberInfoType = columnInfo.MemberInfo.GetMemberInfoType();
                if (columnInfo.ReferenceType == ReferenceType.Many)
                {
                    memberInfoType = memberInfoType.GetGenericArguments().First();
                }

                PocoMemberPlan[] childrenPlans = new PocoMemberPlan[0];
                TableInfoPlan childTableInfoPlan = null;
                List<MemberInfo> members = new List<MemberInfo>(capturedMembers) { columnInfo.MemberInfo };

                if (columnInfo.ComplexMapping || columnInfo.ReferenceType != ReferenceType.None)
                {
                    if (capturedMembers.GroupBy(x => x.GetMemberInfoType()).Any(x => x.Count() >= 2))
                    {
                        continue;
                    }

                    ColumnInfo[] childColumnInfos = this.GetColumnInfos(memberInfoType);

                    if (columnInfo.ReferenceType != ReferenceType.None)
                    {
                        childTableInfoPlan = this.GetTableInfo(memberInfoType, childColumnInfos, members);
                    }

                    string newPrefix = JoinStrings(capturedPrefix, columnInfo.ReferenceType != ReferenceType.None ? "" : (columnInfo.ComplexPrefix ?? columnInfo.MemberInfo.Name));

                    childrenPlans = this.GetPocoMembers(mapper, childColumnInfos, members, newPrefix).ToArray();
                }

                MemberInfo capturedMemberInfo = columnInfo.MemberInfo;
                ColumnInfo capturedColumnInfo = columnInfo;

                List<MemberAccessor> accessors = this.GetMemberAccessors(members);
                Type memberType = capturedMemberInfo.GetMemberInfoType();
                bool isList = IsList(capturedMemberInfo);
                Type listType = GetListType(memberType, isList);
                bool isDynamic = capturedMemberInfo.IsDynamic();
                FastCreate fastCreate = GetFastCreate(memberType, mapper, isList, isDynamic);
                string columnName = this.GetColumnName(capturedPrefix, capturedColumnInfo.ColumnName ?? capturedMemberInfo.Name);
                MemberInfoData memberInfoData = new MemberInfoData(capturedMemberInfo);

                yield return tableInfo =>
                {
                    PocoColumn pc = new PocoColumn
                    {
                        ReferenceType = capturedColumnInfo.ReferenceType,
                        TableInfo = tableInfo,
                        MemberInfoData = memberInfoData,
                        MemberInfoChain = members,
                        ColumnName = columnName,
                        ResultColumn = capturedColumnInfo.ResultColumn,
                        ForceToUtc = capturedColumnInfo.ForceToUtc,
                        ComputedColumn = capturedColumnInfo.ComputedColumn,
                        ComputedColumnType = capturedColumnInfo.ComputedColumnType,
                        ColumnType = capturedColumnInfo.ColumnType,
                        ColumnAlias = capturedColumnInfo.ColumnAlias,
                        VersionColumn = capturedColumnInfo.VersionColumn,
                        VersionColumnType = capturedColumnInfo.VersionColumnType,
                        SerializedColumn = capturedColumnInfo.SerializedColumn
                    };

                    pc.SetMemberAccessors(accessors);

                    TableInfo childrenTableInfo = childTableInfoPlan == null ? tableInfo : childTableInfoPlan();
                    List<PocoMember> children = childrenPlans.Select(plan => plan(childrenTableInfo)).ToList();

                    // Cascade ResultColumn down
                    foreach (PocoMember child in children.Where(child => child.PocoColumn != null && pc.ResultColumn))
                    {
                        child.PocoColumn.ResultColumn = true;
                    }

                    PocoMember pocoMember = new PocoMember()
                    {
                        MemberInfoData = memberInfoData,
                        MemberInfoChain = members,
                        IsList = isList,
                        IsDynamic = isDynamic,
                        PocoColumn = capturedColumnInfo.ComplexMapping ? null : pc,
                        ReferenceType = capturedColumnInfo.ReferenceType,
                        ReferenceMemberName = capturedColumnInfo.ReferenceMemberName,
                        PocoMemberChildren = children,
                    };

                    pocoMember.SetMemberAccessor(accessors[accessors.Count - 1], fastCreate, listType);

                    return pocoMember;
                };
            }
        }

        private static FastCreate GetFastCreate(Type memberType, MapperCollection mapperCollection, bool isList, bool isDynamic)
        {
            return memberType.IsAClass() || isDynamic
                       ? (new FastCreate(isList
                            ? memberType.GetGenericArguments().First()
                            : memberType, mapperCollection))
                       : null;
        }

        private static Type GetListType(Type memberType, bool isList)
        {
            return isList ? typeof(List<>).MakeGenericType(memberType.GetGenericArguments().First()) : null;
        }

        public List<MemberAccessor> GetMemberAccessors(IEnumerable<MemberInfo> memberInfos)
        {
            return memberInfos
                .Select(memberInfo => new MemberAccessor(memberInfo.DeclaringType, memberInfo.Name))
                .ToList();
        }

        public static bool IsList(MemberInfo mi)
        {
            return mi.GetMemberInfoType().IsOfGenericType(typeof(IList<>)) && !mi.GetMemberInfoType().IsArray;
        }

        protected virtual string GetColumnName(string prefix, string columnName)
        {
            return JoinStrings(prefix, columnName);
        }

        public static string JoinStrings(string prefix, string end)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(prefix))
                list.Add(prefix);
            if (!string.IsNullOrEmpty(end))
                list.Add(end);
            return string.Join(PocoData.Separator, list.ToArray());
        }

        protected string CreateAlias(string typeName, Type typeIn)
        {
            string alias;
            int i = 0;
            bool result = false;
            string name = string.Join(string.Empty, typeName.BreakUpCamelCase().Split(' ').Select(x => x.Substring(0, 1)).ToArray());
            do
            {
                alias = name + (i == 0 ? string.Empty : i.ToString());
                i++;

                if (this._aliasToType.AddIfNotExists(alias, typeIn))
                {
                    continue;
                }

                result = true;
            } while (result == false);

            return alias;
        }
    }
}