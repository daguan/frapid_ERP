using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Frapid.NPoco
{
    public class PocoData
    {
        public static string Separator = "__";

        public Type Type { get; private set; }
        public MapperCollection Mapper { get; private set; }

        public KeyValuePair<string, PocoColumn>[] QueryColumns { get; protected internal set; }
        public TableInfo TableInfo { get; protected internal set; }
        public Dictionary<string, PocoColumn> Columns { get; protected internal set; }
        public List<PocoMember> Members { get; protected internal set; }
        public List<PocoColumn> AllColumns { get; protected internal set; }

        public PocoData()
        {
        }

        public PocoData(Type type, MapperCollection mapper) : this()
        {
            this.Type = type;
            this.Mapper = mapper;
        }
        
        public object[] GetPrimaryKeyValues(object obj)
        {
            return this.PrimaryKeyValues(obj);
        }

        public IEnumerable<PocoMember> GetAllMembers()
        {
            return this.GetAllMembers(this.Members);
        }

        private IEnumerable<PocoMember> GetAllMembers(IEnumerable<PocoMember> pocoMembers)
        {
            foreach (PocoMember member in pocoMembers)
            {
                yield return member;
                foreach(PocoMember childmember in this.GetAllMembers(member.PocoMemberChildren))
                {
                    yield return childmember;
                }
            }
        }

        private Func<object, object[]> _primaryKeyValues;
        private Func<object, object[]> PrimaryKeyValues
        {
            get
            {
                if (this._primaryKeyValues == null)
                {
                    string[] multiplePrimaryKeysNames = this.TableInfo.PrimaryKey.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                    IEnumerable<PocoMember> members = multiplePrimaryKeysNames
                        .Select(x => this.Members.FirstOrDefault(y => y.PocoColumn != null
                                && y.ReferenceType == ReferenceType.None
                                && string.Equals(x, y.PocoColumn.ColumnName, StringComparison.OrdinalIgnoreCase)))
                        .Where(x => x != null);
                    this._primaryKeyValues = obj => members.Select(x => x.PocoColumn.GetValue(obj)).ToArray();
                }
                return this._primaryKeyValues;
            }
        }


        public object CreateObject(DbDataReader dataReader)
        {
            if (this.CreateDelegate == null)
                this.CreateDelegate = new FastCreate(this.Type, this.Mapper);
            return this.CreateDelegate.Create(dataReader);
        }

        private FastCreate CreateDelegate { get; set; }

    }
}
