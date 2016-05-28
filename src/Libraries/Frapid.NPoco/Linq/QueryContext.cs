using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Frapid.NPoco.Expressions;

namespace Frapid.NPoco.Linq
{
    public class QueryContext<T>
    {
        private readonly Database _database;
        private readonly PocoData _pocoData;
        private readonly Dictionary<string, JoinData> _joinExpressions;

        public QueryContext(Database database, PocoData pocoData, Dictionary<string, JoinData> joinExpressions)
        {
            this._database = database;
            this._pocoData = pocoData;
            this._joinExpressions = joinExpressions;
        }

        public DatabaseType DatabaseType => this._database.DatabaseType;

        public string GetAliasFor(Expression<Func<T, object>> propertyExpression)
        {
            MemberInfo member = MemberChainHelper.GetMembers(propertyExpression).LastOrDefault();
            if (member == null)
                return this._pocoData.TableInfo.AutoAlias;

            JoinData pocoMember = this._joinExpressions.Values.SingleOrDefault(x => x.PocoMember.MemberInfoData.MemberInfo == member);
            if (pocoMember == null)
                throw new Exception("Tried to get alias for table that has not been included");

            return pocoMember.PocoMemberJoin.PocoColumn.TableInfo.AutoAlias;
        }

        public PocoData GetPocoDataFor<TModel>()
        {
            return this._database.PocoDataFactory.ForType(typeof (TModel));
        }
    }
}