using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Frapid.NPoco.Linq;

namespace Frapid.NPoco.Expressions
{
    public class OrderByMember
    {
        public Type EntityType { get; set; }
        public PocoColumn PocoColumn { get; set; }
        public PocoColumn[] PocoColumns { get; set; }
        public string AscDesc { get; set; }
    }

    public class SelectMember : IEquatable<SelectMember>
    {
        public Type EntityType { get; set; }
        public string SelectSql { get; set; }
        public PocoColumn PocoColumn { get; set; }
        public PocoColumn[] PocoColumns { get; set; }

        public bool Equals(SelectMember other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(this.EntityType, other.EntityType) && Equals(this.PocoColumn, other.PocoColumn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((SelectMember) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.EntityType != null ? this.EntityType.GetHashCode() : 0)*397) ^ (this.PocoColumn != null ? this.PocoColumn.GetHashCode() : 0);
            }
        }
    }

    public class GeneralMember
    {
        public Type EntityType { get; set; }
        public PocoColumn PocoColumn { get; set; }
        public PocoColumn[] PocoColumns { get; set; }
    }

    public interface ISqlExpression
    {
        List<OrderByMember> OrderByMembers { get; }
        int? Rows { get; }
        int? Skip { get; }
        string WhereSql { get; }
        object[] Params { get; }
        Type Type { get; }
        List<SelectMember> SelectMembers { get; }
        List<GeneralMember> GeneralMembers { get; }
        string ApplyPaging(string sql, IEnumerable<PocoColumn[]> columns, Dictionary<string, JoinData> joinSqlExpressions);
    }

    public abstract class SqlExpression<T> : ISqlExpression
    {
        private Expression<Func<T, bool>> underlyingExpression;
        private List<string> orderByProperties = new List<string>();
        private List<OrderByMember> orderByMembers = new List<OrderByMember>();
        private List<SelectMember> selectMembers = new List<SelectMember>();
        private List<GeneralMember> generalMembers = new List<GeneralMember>();
        private string selectExpression = string.Empty;
        private string whereExpression;
        private string groupBy = string.Empty;
        private string havingExpression;
        private string orderBy = string.Empty;

        List<OrderByMember> ISqlExpression.OrderByMembers => this.orderByMembers;
        List<SelectMember> ISqlExpression.SelectMembers => this.selectMembers;
        List<GeneralMember> ISqlExpression.GeneralMembers => this.generalMembers;
        string ISqlExpression.WhereSql => this.whereExpression;
        int? ISqlExpression.Rows => this.Rows;
        int? ISqlExpression.Skip => this.Skip;
        Type ISqlExpression.Type => this._type;
        object[] ISqlExpression.Params => this.Context.Params;

        string ISqlExpression.ApplyPaging(string sql, IEnumerable<PocoColumn[]> columns, Dictionary<string, JoinData> joinSqlExpressions)
        {
            return this.ApplyPaging(sql, columns, joinSqlExpressions);
        }

        private string sep = string.Empty;
        private PocoData _pocoData;
        private readonly IDatabase _database;
        private readonly DatabaseType _databaseType;
        private bool PrefixFieldWithTableName { get; set; }
        private bool WhereStatementWithoutWhereString { get; set; }
        private Type _type { get; set; }

        public SqlExpression(IDatabase database, PocoData pocoData, bool prefixTableName)
        {
            this._type = typeof(T);
            this._pocoData = pocoData;
            this._database = database;
            this._databaseType = database.DatabaseType;
            this.PrefixFieldWithTableName = prefixTableName;
            this.WhereStatementWithoutWhereString = false;
            this.paramPrefix = "@";
            this.Context = new SqlExpressionContext(this);
        }

        public class SqlExpressionContext
        {
            private readonly SqlExpression<T> _expression;

            public SqlExpressionContext(SqlExpression<T> expression)
            {
                this._expression = expression;
                this.UpdateFields = new List<string>();
            }

            public List<string> UpdateFields { get; set; }
            public object[] Params => this._expression._params.ToArray();

            public virtual string ToDeleteStatement()
            {
                return this._expression.ToDeleteStatement();
            }

            public virtual string ToUpdateStatement(T item)
            {
                return this._expression.ToUpdateStatement(item, false);
            }

            public virtual string ToUpdateStatement(T item, bool excludeDefaults)
            {
                return this._expression.ToUpdateStatement(item, excludeDefaults);
            }

            public virtual string ToUpdateStatement(T item, bool excludeDefaults, bool allFields)
            {
                if (allFields)
                    this._expression.generalMembers = this._expression.GetAllMembers().Select(x => new GeneralMember { EntityType = typeof(T), PocoColumn = x }).ToList();

                return this._expression.ToUpdateStatement(item, excludeDefaults);
            }

            public string ToWhereStatement()
            {
                return this._expression.ToWhereStatement();
            }

            public virtual string ToSelectStatement()
            {
                return this.ToSelectStatement(true, false);
            }

            public virtual string ToSelectStatement(bool applyPaging, bool distinct)
            {
                return this._expression.ToSelectStatement(applyPaging, distinct);
            }
        }

        /// <summary>
        /// Clear select expression. All properties will be selected.
        /// </summary>
        //public virtual SqlExpression<T> Select()
        //{
        //    return Select(string.Empty);
        //}

        /// <summary>
        /// set the specified selectExpression.
        /// </summary>
        /// <param name='selectExpression'>
        /// raw Select expression: "Select SomeField1, SomeField2 from SomeTable"
        /// </param>
        //public virtual SqlExpression<T> Select(string selectExpression)
        //{
        //    if (!string.IsNullOrEmpty(selectExpression))
        //    {
        //        this.selectExpression = selectExpression;
        //    }
        //    return this;
        //}

        /// <summary>
        /// Fields to be selected.
        /// </summary>
        /// <param name='fields'>
        /// x=> x.SomeProperty1 or x=> new{ x.SomeProperty1, x.SomeProperty2}
        /// </param>
        /// <typeparam name='TKey'>
        /// objectWithProperties
        /// </typeparam>
        public virtual SqlExpression<T> Select<TKey>(Expression<Func<T, TKey>> fields)
        {
            this.sep = string.Empty;
            this.selectMembers.Clear();
            this.Visit(fields);
            return this;
        }

        public virtual List<SelectMember> SelectProjection<TKey>(Expression<Func<T, TKey>> fields)
        {
            this.sep = string.Empty;
            this.selectMembers.Clear();
            this._projection = true;
            Expression exp = PartialEvaluator.Eval(fields, this.CanBeEvaluatedLocally);
            this.Visit(exp);
            this._projection = false;
            List<SelectMember> proj = this.selectMembers.Union(this.generalMembers.Select(x=>new SelectMember() { EntityType = x.EntityType, PocoColumn = x.PocoColumn, PocoColumns = x.PocoColumns })).ToList();
            this.selectMembers.Clear();
            return proj;
        }

        public virtual List<SelectMember> SelectDistinct<TKey>(Expression<Func<T, TKey>> fields)
        {
            return this.SelectProjection(fields);
        }

        //public virtual SqlExpression<T> Where()
        //{
        //    if (underlyingExpression != null) underlyingExpression = null; //Where() clears the expression
        //    return Where(string.Empty);
        //}

        public virtual SqlExpression<T> Where(string sqlFilter, params object[] filterParams)
        {
            if (string.IsNullOrEmpty(sqlFilter))
                return this;

            sqlFilter = ParameterHelper.ProcessParams(sqlFilter, filterParams, this._params);

            if (string.IsNullOrEmpty(this.whereExpression))
            {
                this.whereExpression = "WHERE " + sqlFilter;
            }
            else
            {
                this.whereExpression += " AND " + sqlFilter;
            }

            return this;
        }

        public string On<T2>(Expression<Func<T, T2, bool>> predicate)
        {
            this.sep = " ";
            string onSql = this.Visit(predicate).ToString();
            return onSql;
        }

        public virtual SqlExpression<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                this.And(predicate);
            }
            else
            {
                this.underlyingExpression = null;
                this.whereExpression = string.Empty;
            }

            return this;
        }

        protected virtual SqlExpression<T> And(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                if (this.underlyingExpression == null)
                    this.underlyingExpression = predicate;
                else
                    this.underlyingExpression = this.underlyingExpression.And(predicate);

                this.ProcessInternalExpression();
            }
            return this;
        }

        //public virtual SqlExpression<T> Or(Expression<Func<T, bool>> predicate)
        //{
        //    if (predicate != null)
        //    {
        //        if (underlyingExpression == null)
        //            underlyingExpression = predicate;
        //        else
        //            underlyingExpression = underlyingExpression.Or(predicate);

        //        ProcessInternalExpression();
        //    }
        //    return this;
        //}

        private void ProcessInternalExpression()
        {
            this.sep = " ";
            Expression exp = PartialEvaluator.Eval(this.underlyingExpression, this.CanBeEvaluatedLocally);
            this.whereExpression = this.Visit(exp).ToString();
            if (!string.IsNullOrEmpty(this.whereExpression)) this.whereExpression = (this.WhereStatementWithoutWhereString ? "" : "WHERE ") + this.whereExpression;
        }

        private bool CanBeEvaluatedLocally(Expression expression)
        {
            // any operation on a query can't be done locally
            ConstantExpression cex = expression as ConstantExpression;
            if (cex != null)
            {
                IQueryable query = cex.Value as IQueryable;
                if (query != null && query.Provider == this)
                    return false;
            }
            MethodCallExpression mc = expression as MethodCallExpression;
            if (mc != null &&
                (mc.Method.DeclaringType == typeof(Enumerable) ||
                 mc.Method.DeclaringType == typeof(Queryable)))
            {
                return false;
            }
            if (expression.NodeType == ExpressionType.Convert &&
                expression.Type == typeof(object))
                return true;
            return expression.NodeType != ExpressionType.Parameter &&
                   expression.NodeType != ExpressionType.Lambda;
        }

        //public virtual SqlExpression<T> GroupBy()
        //{
        //    return GroupBy(string.Empty);
        //}

        //public virtual SqlExpression<T> GroupBy(string groupBy)
        //{
        //    this.groupBy = groupBy;
        //    return this;
        //}

        public virtual SqlExpression<T> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.sep = string.Empty;
            this.groupBy = this.Visit(keySelector).ToString();
            if (!string.IsNullOrEmpty(this.groupBy)) this.groupBy = string.Format("GROUP BY {0}", this.groupBy);
            return this;
        }

        //public virtual SqlExpression<T> Having()
        //{
        //    return Having(string.Empty);
        //}

        //public virtual SqlExpression<T> Having(string sqlFilter, params object[] filterParams)
        //{
        //    havingExpression = !string.IsNullOrEmpty(sqlFilter) ? sqlFilter : string.Empty;
        //    foreach (var filterParam in filterParams)
        //    {
        //        CreateParam(filterParam);
        //    }
        //    if (!string.IsNullOrEmpty(havingExpression)) havingExpression = "HAVING " + havingExpression;
        //    return this;
        //}

        //public virtual SqlExpression<T> Having(Expression<Func<T, bool>> predicate)
        //{

        //    if (predicate != null)
        //    {
        //        sep = " ";
        //        havingExpression = Visit(predicate).ToString();
        //        if (!string.IsNullOrEmpty(havingExpression)) havingExpression = "HAVING " + havingExpression;
        //    }
        //    else
        //        havingExpression = string.Empty;

        //    return this;
        //}



        //public virtual SqlExpression<T> OrderBy()
        //{
        //    return OrderBy(string.Empty);
        //}

        //public virtual SqlExpression<T> OrderBy(string orderBy)
        //{
        //    orderByProperties.Clear();
        //    this.orderBy = orderBy;
        //    return this;
        //}

        public virtual SqlExpression<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.sep = string.Empty;
            this.orderByProperties.Clear();
            this.orderByMembers.Clear();
            this.generalMembers.Clear();
            MemberAccessString memberAccess = (MemberAccessString)this.Visit(keySelector);
            this.orderByProperties.Add(memberAccess + " ASC");
            this.orderByMembers.Add(new OrderByMember { AscDesc = "ASC", PocoColumn = memberAccess.PocoColumn, EntityType = memberAccess.Type, PocoColumns = memberAccess.PocoColumns });
            this.generalMembers.Clear();
            this.BuildOrderByClauseInternal();
            return this;
        }

        public virtual SqlExpression<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.sep = string.Empty;
            this.generalMembers.Clear();
            MemberAccessString memberAccess = (MemberAccessString)this.Visit(keySelector);
            this.orderByProperties.Add(memberAccess + " ASC");
            this.orderByMembers.Add(new OrderByMember { AscDesc = "ASC", PocoColumn = memberAccess.PocoColumn, EntityType = memberAccess.Type, PocoColumns = memberAccess.PocoColumns });
            this.generalMembers.Clear();
            this.BuildOrderByClauseInternal();
            return this;
        }

        public virtual SqlExpression<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.sep = string.Empty;
            this.orderByProperties.Clear();
            this.orderByMembers.Clear();
            this.generalMembers.Clear();
            MemberAccessString memberAccess = (MemberAccessString)this.Visit(keySelector);
            this.orderByProperties.Add(memberAccess + " DESC");
            this.orderByMembers.Add(new OrderByMember { AscDesc = "DESC", PocoColumn = memberAccess.PocoColumn, EntityType = memberAccess.Type, PocoColumns = memberAccess.PocoColumns });
            this.generalMembers.Clear();
            this.BuildOrderByClauseInternal();
            return this;
        }

        public virtual SqlExpression<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.sep = string.Empty;
            this.generalMembers.Clear();
            MemberAccessString memberAccess = (MemberAccessString)this.Visit(keySelector);
            this.orderByProperties.Add(memberAccess + " DESC");
            this.orderByMembers.Add(new OrderByMember { AscDesc = "DESC", PocoColumn = memberAccess.PocoColumn, EntityType = memberAccess.Type, PocoColumns = memberAccess.PocoColumns });
            this.generalMembers.Clear();
            this.BuildOrderByClauseInternal();
            return this;
        }

        private void BuildOrderByClauseInternal()
        {
            if (this.orderByMembers.Count > 0)
            {
                this.orderBy = "ORDER BY " + string.Join(", ", this.orderByMembers.Select(x => (this.PrefixFieldWithTableName ? this._databaseType.EscapeSqlIdentifier(x.PocoColumns.Last().MemberInfoKey) : this._databaseType.EscapeSqlIdentifier(x.PocoColumns.Last().MemberInfoKey)) + " " + x.AscDesc).ToArray());
            }
            else
            {
                this.orderBy = null;
            }
        }


        /// <summary>
        /// Set the specified offset and rows for SQL Limit clause.
        /// </summary>
        /// <param name='skip'>
        /// Offset of the first row to return. The offset of the initial row is 0
        /// </param>
        /// <param name='rows'>
        /// Number of rows returned by a SELECT statement
        /// </param>
        public virtual SqlExpression<T> Limit(int skip, int rows)
        {
            this.Rows = rows;
            this.Skip = skip;
            return this;
        }

        /// <summary>
        /// Set the specified rows for Sql Limit clause.
        /// </summary>
        /// <param name='rows'>
        /// Number of rows returned by a SELECT statement
        /// </param>
        public virtual SqlExpression<T> Limit(int rows)
        {
            this.Rows = rows;
            this.Skip = 0;
            return this;
        }

        /// <summary>
        /// Clear Sql Limit clause
        /// </summary>
        //public virtual SqlExpression<T> Limit()
        //{
        //    Skip = null;
        //    Rows = null;
        //    return this;
        //}


        /// <summary>
        /// Fields to be updated.
        /// </summary>
        /// <param name='updatefields'>
        /// IList<string> containing Names of properties to be updated
        /// </param>
        //public virtual SqlExpression<T> Update(IList<string> updateFields)
        //{
        //    this.updateFields = updateFields;
        //    return this;
        //}

        /// <summary>
        /// Fields to be updated.
        /// </summary>
        /// <param name='fields'>
        /// x=> x.SomeProperty1 or x=> new{ x.SomeProperty1, x.SomeProperty2}
        /// </param>
        /// <typeparam name='TKey'>
        /// objectWithProperties
        /// </typeparam>
        public virtual SqlExpression<T> Update<TKey>(Expression<Func<T, TKey>> fields)
        {
            this.sep = string.Empty;
            this.generalMembers.Clear();
            this.Visit(fields);
            this.Context.UpdateFields = new List<string>(this.generalMembers.Select(x => x.PocoColumn.MemberInfoData.Name));
            this.generalMembers.Clear();
            return this;
        }

        /// <summary>
        /// Clear UpdateFields list ( all fields will be updated)
        /// </summary>
        //public virtual SqlExpression<T> Update()
        //{
        //    this.updateFields = new List<string>();
        //    return this;
        //}

        /// <summary>
        /// Fields to be inserted.
        /// </summary>
        /// <param name='fields'>
        /// x=> x.SomeProperty1 or x=> new{ x.SomeProperty1, x.SomeProperty2}
        /// </param>
        /// <typeparam name='TKey'>
        /// objectWithProperties
        /// </typeparam>
        //public virtual SqlExpression<T> Insert<TKey>(Expression<Func<T, TKey>> fields)
        //{
        //    sep = string.Empty;
        //    Context.InsertFields = Visit(fields).ToString().Split(',').ToList();
        //    return this;
        //}

        /// <summary>
        /// fields to be inserted.
        /// </summary>
        /// <param name='insertFields'>
        /// IList&lt;string&gt; containing Names of properties to be inserted
        /// </param>
        //public virtual SqlExpression<T> Insert(IList<string> insertFields)
        //{
        //    this.insertFields = insertFields;
        //    return this;
        //}

        /// <summary>
        /// Clear InsertFields list ( all fields will be inserted)
        /// </summary>
        //public virtual SqlExpression<T> Insert()
        //{
        //    this.insertFields = new List<string>();
        //    return this;
        //}

        protected virtual string ToDeleteStatement()
        {
            return string.Format("DELETE {0} FROM {1} {2}",
                (this.PrefixFieldWithTableName ? this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias) : string.Empty),
                this._databaseType.EscapeTableName(this._pocoData.TableInfo.TableName) + (this.PrefixFieldWithTableName ? " " + this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias) : string.Empty),
                this.WhereExpression);
        }

        protected virtual string ToUpdateStatement(T item)
        {
            return this.ToUpdateStatement(item, false);
        }

        protected virtual string ToUpdateStatement(T item, bool excludeDefaults)
        {
            StringBuilder setFields = new StringBuilder();

            foreach (KeyValuePair<string, PocoColumn> fieldDef in this._pocoData.Columns)
            {
                if (this.Context.UpdateFields.Count > 0 && !this.Context.UpdateFields.Contains(fieldDef.Value.MemberInfoData.Name)) continue; // added
                object value = fieldDef.Value.GetValue(item);
                if (this._database.Mappers != null)
                {
                    value = this._database.Mappers.FindAndExecute(x => x.GetToDbConverter(fieldDef.Value.ColumnType, fieldDef.Value.MemberInfoData.MemberInfo), value);
                }

                if (excludeDefaults && (value == null || value.Equals(MappingHelper.GetDefault(value.GetType())))) continue; //GetDefaultValue?

                if (setFields.Length > 0)
                    setFields.Append(", ");

                setFields.AppendFormat("{0} = {1}", (this.PrefixFieldWithTableName ? this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias) + "." : string.Empty) + this._databaseType.EscapeSqlIdentifier(fieldDef.Value.ColumnName), this.CreateParam(value));
            }

            if (this.PrefixFieldWithTableName)
                return string.Format("UPDATE {0} SET {2} FROM {1} {3}", this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias), this._databaseType.EscapeTableName(this._pocoData.TableInfo.TableName) + " " + this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias), setFields, this.WhereExpression);
            else
                return string.Format("UPDATE {0} SET {1} {2}", this._databaseType.EscapeTableName(this._pocoData.TableInfo.TableName), setFields, this.WhereExpression);
        }

        protected string ToWhereStatement()
        {
            return this.WhereExpression;
        }

        protected virtual string ToSelectStatement(bool applyPaging, bool isDistinct)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(this.GetSelectExpression(isDistinct));
            sql.Append(string.IsNullOrEmpty(this.WhereExpression) ?
                       "" :
                       " \n" + this.WhereExpression);
            sql.Append(string.IsNullOrEmpty(this.GroupByExpression) ?
                       "" :
                       " \n" + this.GroupByExpression);
            sql.Append(string.IsNullOrEmpty(this.HavingExpression) ?
                       "" :
                       " \n" + this.HavingExpression);
            sql.Append(string.IsNullOrEmpty(this.OrderByExpression) ?
                       "" :
                       " \n" + this.OrderByExpression);

            return applyPaging ? this.ApplyPaging(sql.ToString(), this.ModelDef.QueryColumns.Select(x=> new[] { x.Value }), new Dictionary<string, JoinData>()) : sql.ToString();
        }

        //public virtual string ToCountStatement()
        //{
        //    return OrmLiteConfig.DialectProvider.ToCountStatement(modelDef.ModelType, WhereExpression, null);
        //}

        private string GetSelectExpression(bool distinct)
        {
            IEnumerable<SelectMember> selectMembersFromOrderBys = this.orderByMembers
                .Select(x => new SelectMember() { PocoColumn = x.PocoColumn, EntityType = x.EntityType, PocoColumns = new[] { x.PocoColumn }})
                .Where(x => !this.selectMembers.Any(y => y.EntityType == x.EntityType && y.PocoColumn.MemberInfoData.Name == x.PocoColumn.MemberInfoData.Name));

            IEnumerable<SelectMember> morecols = this.selectMembers.Concat(selectMembersFromOrderBys);
            List<SelectMember> cols = this.selectMembers.Count == 0 ? null : morecols.ToList();
            string selectsql = this.BuildSelectExpression(cols, distinct);
            return selectsql;
        }

        private string WhereExpression
        {
            get
            {
                return this.whereExpression;
            }
            set
            {
                this.whereExpression = value;
            }
        }

        private string GroupByExpression
        {
            get
            {
                return this.groupBy;
            }
            set
            {
                this.groupBy = value;
            }
        }

        private string HavingExpression
        {
            get
            {
                return this.havingExpression;
            }
            set
            {
                this.havingExpression = value;
            }
        }


        private string OrderByExpression
        {
            get
            {
                return this.orderBy;
            }
            set
            {
                this.orderBy = value;
            }
        }

        protected virtual string LimitExpression
        {
            get
            {
                if (!this.Skip.HasValue) return "";
                string rows;
                if (this.Rows.HasValue)
                {
                    rows = string.Format(",{0}", this.Rows.Value);
                }
                else
                {
                    rows = string.Empty;
                }
                return string.Format("LIMIT {0}{1}", this.Skip.Value, rows);
            }
        }

        private int? Rows { get; set; }
        private int? Skip { get; set; }

        protected internal PocoData ModelDef
        {
            get
            {
                return this._pocoData;
            }
            set
            {
                this._pocoData = value;
            }
        }

        protected internal virtual object Visit(Expression exp)
        {

            if (exp == null) return string.Empty;
            switch (exp.NodeType)
            {
                case ExpressionType.Lambda:
                    return this.VisitLambda(exp as LambdaExpression);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess(exp as MemberExpression);
                case ExpressionType.Constant:
                    return this.VisitConstant(exp as ConstantExpression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    //return "(" + VisitBinary(exp as BinaryExpression) + ")";
                    return this.VisitBinary(exp as BinaryExpression);
                case ExpressionType.Conditional:
                    return this.VisitConditional(exp as ConditionalExpression);
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary(exp as UnaryExpression);
                case ExpressionType.Parameter:
                    return this.VisitParameter(exp as ParameterExpression);
                case ExpressionType.Call:
                    return this.VisitMethodCall(exp as MethodCallExpression);
                case ExpressionType.New:
                    return this.VisitNew(exp as NewExpression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray(exp as NewArrayExpression);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                default:
                    return exp.ToString();
            }
        }
        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = init.NewExpression;
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
            if (n != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }
            return init;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            for (int i = 0, n = original.Count; i < n; i++)
            {
                this.VisitBinding(original[i]);
            }
            return original;
        }

        protected virtual object VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                //case MemberBindingType.ListBinding:
                //    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual object VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            return this.VisitBindingList(binding.Bindings);
        }

        protected virtual object VisitMemberAssignment(MemberAssignment assignment)
        {
            return this.Visit(assignment.Expression);
        }

        protected virtual object VisitLambda(LambdaExpression lambda)
        {
            if (lambda.Body.NodeType == ExpressionType.MemberAccess && this.sep == " ")
            {
                MemberExpression m = lambda.Body as MemberExpression;

                if (m.Expression != null)
                {
                    if (IsNullableMember(m))
                    {
                        string r = this.VisitMemberAccess(m.Expression as MemberExpression).ToString();
                        return string.Format("{0} is not null", r);
                    }
                    else
                    {
                        string r = this.VisitMemberAccess(m).ToString();
                        return string.Format("{0}={1}", r, this.GetQuotedTrueValue());
                    }
                }
            }
            else if (lambda.Body.NodeType == ExpressionType.Constant)
            {
                object result = this.Visit(lambda.Body);
                if (result is bool)
                {
                    return ((bool) result) ? "1=1" : "1<>1";
                }
            }
            return this.Visit(lambda.Body);
        }

        private static bool IsNullableMember(MemberExpression m)
        {
            MemberExpression member = m.Expression as MemberExpression;
            return member != null
                && (m.Member.Name == "HasValue")
                && member.Type.GetTypeInfo().IsGenericType && member.Type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        protected virtual object VisitBinary(BinaryExpression b)
        {
            object left, right;
            string operand = this.BindOperant(b.NodeType);   //sep= " " ??
            if (operand == "AND" || operand == "OR")
            {
                MemberExpression m = b.Left as MemberExpression;
                if (m != null && m.Expression != null
                    && m.Expression.NodeType == ExpressionType.Parameter)
                    left = new PartialSqlString(string.Format("{0} = {1}", this.VisitMemberAccess(m), this.GetQuotedTrueValue()));
                else
                    left = this.Visit(b.Left);

                m = b.Right as MemberExpression;
                if (m != null && m.Expression != null
                    && m.Expression.NodeType == ExpressionType.Parameter)
                    right = new PartialSqlString(string.Format("{0} = {1}", this.VisitMemberAccess(m), this.GetQuotedTrueValue()));
                else
                    right = this.Visit(b.Right);

                if (left as PartialSqlString == null && right as PartialSqlString == null)
                {
                    object result = Expression.Lambda(b).Compile().DynamicInvoke();
                    return new PartialSqlString(this.CreateParam(result));
                }

                if (left as PartialSqlString == null)
                    left = ((bool)left) ? this.GetTrueExpression() : this.GetFalseExpression();
                if (right as PartialSqlString == null)
                    right = ((bool)right) ? this.GetTrueExpression() : this.GetFalseExpression();
            }
            else
            {
                left = this.Visit(b.Left);
                right = this.Visit(b.Right);

                if (left as EnumMemberAccess != null && right as PartialSqlString == null)
                {
                    PocoColumn pc = ((EnumMemberAccess)left).PocoColumn;

                    long numvericVal;
                    if (pc.ColumnType == typeof(string))
                        right = this.CreateParam(Enum.Parse(GetMemberInfoTypeForEnum(pc), right.ToString()).ToString());
                    else if (Int64.TryParse(right.ToString(), out numvericVal))
                        right = this.CreateParam(Enum.ToObject(GetMemberInfoTypeForEnum(pc), numvericVal));
                    else
                        right = this.CreateParam(right);
                }
                else if (left as NullableMemberAccess != null && right as PartialSqlString == null)
                {
                    operand = ((bool)right) ? "is not" : "is";
                    right = new PartialSqlString("null");
                }
                else if (right as EnumMemberAccess != null && left as PartialSqlString == null)
                {
                    PocoColumn pc = ((EnumMemberAccess)right).PocoColumn;

                    //enum value was returned by Visit(b.Left)
                    long numvericVal;
                    if (pc.ColumnType == typeof(string))
                        left = this.CreateParam(Enum.Parse(GetMemberInfoTypeForEnum(pc), left.ToString()).ToString());
                    else if (Int64.TryParse(left.ToString(), out numvericVal))
                        left = this.CreateParam(Enum.ToObject(GetMemberInfoTypeForEnum(pc), numvericVal));
                    else
                        left = this.CreateParam(left);
                }
                else if (left as MemberAccessString != null
                    && right is int
                    && new [] { typeof(char), typeof(char?) }.Contains(((MemberAccessString)left).PocoColumn.MemberInfoData.MemberType))
                {
                    right = this.CreateParam(Convert.ToChar(right));
                }
                else if (left as MemberAccessString != null
                    && right is string
                    && ((MemberAccessString) left).PocoColumn.ColumnType == typeof (AnsiString))
                {
                    right = this.CreateParam(new AnsiString((string)right));
                }
                else if (left as PartialSqlString == null && right as PartialSqlString == null)
                {
                    object result = Expression.Lambda(b).Compile().DynamicInvoke();
                    return result;
                }
                else if (left as PartialSqlString == null)
                    left = this.CreateParam(left);
                else if (right as PartialSqlString == null)
                    right = this.CreateParam(right);

            }

            if (operand == "=" && right.ToString().Equals("null", StringComparison.OrdinalIgnoreCase)) operand = "is";
            else if (operand == "<>" && right.ToString().Equals("null", StringComparison.OrdinalIgnoreCase)) operand = "is not";

            switch (operand)
            {
                case "MOD":
                case "COALESCE":
                    return new PartialSqlString(string.Format("{0}({1},{2})", operand, left, right));
                default:
                    return new PartialSqlString("(" + left + this.sep + operand + this.sep + right + ")");
            }
        }

        private static Type GetMemberInfoTypeForEnum(PocoColumn pc)
        {
            if (pc.MemberInfoData.MemberType.GetTypeInfo().IsEnum)
                return pc.MemberInfoData.MemberType;

            return Nullable.GetUnderlyingType(pc.MemberInfoData.MemberType);
        }

        protected virtual object VisitMemberAccess(MemberExpression m)
        {
            bool isNull = false;

            if (IsNullableMember(m))
            {
                m = m.Expression as MemberExpression;
                isNull = true;
            }

            if (m.Expression != null
                && (m.Expression.NodeType == ExpressionType.Parameter
                    || m.Expression.NodeType == ExpressionType.Convert
                    || m.Expression.NodeType == ExpressionType.MemberAccess))
            {
                MemberInfo[] propertyInfos = MemberChainHelper.GetMembers(m).ToArray();
                Type type = this.GetCorrectType(m);

                PocoMember[] pocoMembers = this.ModelDef.GetAllMembers()
                    .Where(x => x.MemberInfoChain.Select(y => y.Name).SequenceEqual(propertyInfos.Select(y => y.Name)))
                    .ToArray();

                PocoMember pocoMember = pocoMembers.LastOrDefault();
                if (pocoMember == null)
                {
                    throw new Exception(
                        string.Format("Did you forget to include the property eg. Include(x => x.{0})",
                        string.Join(".", propertyInfos.Select(y => y.Name).Take(propertyInfos.Length - 1).ToArray())));
                }

                if (this._projection &&
                    (pocoMember.ReferenceType == ReferenceType.Foreign
                    || pocoMember.ReferenceType == ReferenceType.OneToOne)
                    || pocoMember.PocoColumn == null)
                {
                    foreach (PocoMember member in pocoMember.PocoMemberChildren.Where(x => x.PocoColumn != null))
                    {
                        this.generalMembers.Add(new GeneralMember()
                        {
                            EntityType = pocoMember.MemberInfoData.MemberType,
                            PocoColumn = member.PocoColumn,
                            PocoColumns = new [] { member.PocoColumn }
                        });
                    }

                    return new PartialSqlString("");
                }

                PocoColumn pocoColumn = pocoMember.PocoColumn;
                PocoColumn[] pocoColumns = pocoMembers.Select(x => x.PocoColumn).ToArray();

                string columnName = (this.PrefixFieldWithTableName
                                          ? this._databaseType.EscapeTableName(pocoColumn.TableInfo.AutoAlias) + "."
                                          : "")
                                     + this._databaseType.EscapeSqlIdentifier(pocoColumn.ColumnName);

                this.generalMembers.Add(new GeneralMember()
                {
                    EntityType = type,
                    PocoColumn = pocoColumn,
                    PocoColumns = pocoColumns
                });

                if (isNull)
                    return new NullableMemberAccess(pocoColumn, pocoColumns, columnName, type);

                if (Database.IsEnum(pocoColumn.MemberInfoData))
                    return new EnumMemberAccess(pocoColumn, pocoColumns, columnName, type);

                return new MemberAccessString(pocoColumn, pocoColumns, columnName, type);
            }

            UnaryExpression memberExp = Expression.Convert(m, typeof(object));
            Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(memberExp);
            Func<object> getter = lambda.Compile();
            return getter();
        }

        private Type GetCorrectType(MemberExpression m)
        {
            Type type = m.Member.DeclaringType;
            if (m.Expression.NodeType == ExpressionType.MemberAccess)
            {
                type = ((PropertyInfo)((MemberExpression)m.Expression).Member).PropertyType;
            }
            else if (m.Expression.NodeType == ExpressionType.Parameter)
            {
                type = m.Expression.Type;
            }
            return type;
        }

        protected virtual object VisitNew(NewExpression nex)
        {
            UnaryExpression member = Expression.Convert(nex, typeof(object));
            Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(member);
            try
            {
                Func<object> getter = lambda.Compile();
                return getter();
            }
            catch (System.InvalidOperationException)
            {
                List<PartialSqlString> exprs = this.VisitExpressionList(nex.Arguments).OfType<PartialSqlString>().ToList();
                StringBuilder r = new StringBuilder();
                for (int i = 0; i < exprs.Count; i++)
                {
                    if (exprs[i] is MemberAccessString)
                    {
                        this.selectMembers.Add(new SelectMember()
                        {
                            EntityType = ((MemberAccessString)exprs[i]).Type,
                            PocoColumn = ((MemberAccessString)exprs[i]).PocoColumn,
                            PocoColumns = ((MemberAccessString)exprs[i]).PocoColumns,
                        });
                    }
                }
                return r.ToString();
            }

        }

        protected virtual object VisitParameter(ParameterExpression p)
        {
            return p.Name;
        }

        List<object> _params = new List<object>();
        int _paramCounter = 0;
        string paramPrefix;
        private bool _projection;
        public SqlExpressionContext Context { get; private set; }

        protected virtual object VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
                return new PartialSqlString("null");

            return c.Value;
        }

        protected virtual object VisitConditional(ConditionalExpression conditional)
        {
            this.sep = " ";
            object test = this.Visit(conditional.Test);
            object trueSql = this.Visit(conditional.IfTrue);
            object falseSql = this.Visit(conditional.IfFalse);

            return new PartialSqlString(string.Format("(case when {0} then {1} else {2} end)", test, trueSql, falseSql));
        }

        protected string CreateParam(object value)
        {
            string paramPlaceholder = this.paramPrefix + this._paramCounter++;
            this._params.Add(value);
            return paramPlaceholder;
        }

        protected virtual object VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    object o = this.Visit(u.Operand);

                    if (o as PartialSqlString == null)
                        return !((bool)o);

                    if (o as MemberAccessString != null)
                    {
                        if (o as NullableMemberAccess != null)
                            o = o + " is not null";
                        else
                            o = o + " = " + this.GetQuotedTrueValue();
                    }

                    return new PartialSqlString("NOT (" + o + ")");
                case ExpressionType.Convert:
                    if (u.Method != null)
                        return Expression.Lambda(u).Compile().DynamicInvoke();
                    break;
            }

            return this.Visit(u.Operand);

        }

        private bool IsColumnAccess(MethodCallExpression m)
        {
            if (m.Object != null && m.Object as MethodCallExpression != null)
                return this.IsColumnAccess(m.Object as MethodCallExpression);

            MemberExpression exp = m.Object as MemberExpression;
            return exp != null
                && exp.Expression != null
                && ((exp.Expression.Type == typeof(T) && exp.Expression.NodeType == ExpressionType.Parameter
                    || exp.Expression.NodeType == ExpressionType.MemberAccess));
        }

        protected virtual object VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(S))
                return this.VisitSqlMethodCall(m);

            if (this.IsStaticArrayMethod(m))
                return this.VisitStaticArrayMethodCall(m);

            if (this.IsEnumerableMethod(m))
                return this.VisitEnumerableMethodCall(m);

            if (this.IsColumnAccess(m))
                return this.VisitColumnAccessMethod(m);

            if (this._projection && this.VisitInnerMethodCall(m))
                return null;

            return Expression.Lambda(m).Compile().DynamicInvoke();
        }

        private bool VisitInnerMethodCall(MethodCallExpression m)
        {
            bool found = false;
            if (m.Arguments.Any(args => this.ProcessMethodSearchRecursively(args, ref found)))
            {
                return true;
            }
            return found;
        }

        private bool ProcessMethodSearchRecursively(Expression args, ref bool found)
        {
            if (args.NodeType == ExpressionType.Parameter && args.Type == typeof (T))
            {
                this.selectMembers.AddRange(this._pocoData.QueryColumns.Select(x => new SelectMember { PocoColumn = x.Value, EntityType = this._pocoData.Type, PocoColumns = new[] { x.Value } }));
                return true;
            }

            IEnumerable<Expression> nestedExpressions = null;
            MethodCallExpression nested1 = args as MethodCallExpression;
            if (nested1 != null)
            {
                nestedExpressions = nested1.Arguments;
            }
            else
            {
                NewArrayExpression nested2 = args as NewArrayExpression;
                if (nested2 != null) nestedExpressions = nested2.Expressions;
            }

            if (nestedExpressions != null)
            {
                foreach (Expression nestedExpression in nestedExpressions)
                {
                    if (this.ProcessMethodSearchRecursively(nestedExpression, ref found))
                        return true;
                }
            }

            MemberAccessString result = this.Visit(args) as MemberAccessString;
            found = found || result != null;

            return false;
        }

        private bool IsStaticArrayMethod(MethodCallExpression m)
        {
            if (m.Object == null && m.Method.Name == "Contains")
            {
                return m.Arguments.Count == 2;
            }

            return false;
        }

        private bool IsEnumerableMethod(MethodCallExpression m)
        {
            if (m.Object != null
                && m.Object.Type.IsOrHasGenericInterfaceTypeOf(typeof(IEnumerable<>))
                && m.Object.Type != typeof(string)
                && m.Method.Name == "Contains")
            {
                return m.Arguments.Count == 1;
            }

            return false;
        }

        protected virtual object VisitEnumerableMethodCall(MethodCallExpression m)
        {
            switch (m.Method.Name)
            {
                case "Contains":
                    List<object> args = this.VisitExpressionList(m.Arguments);
                    return new PartialSqlString(this.BuildInStatement(m.Object, args[0]));

                default:
                    throw new NotSupportedException();
            }
        }

        protected virtual object VisitStaticArrayMethodCall(MethodCallExpression m)
        {
            switch (m.Method.Name)
            {
                case "Contains":
                    List<object> args = this.VisitExpressionList(m.Arguments);
                    Expression memberExpr = m.Arguments[0];
                    if (memberExpr.NodeType == ExpressionType.MemberAccess)
                        memberExpr = (m.Arguments[0] as MemberExpression);

                    return new PartialSqlString(this.BuildInStatement(memberExpr, args[1]));

                default:
                    throw new NotSupportedException();
            }
        }

        private StringBuilder FlattenList(IEnumerable inArgs, object partialSqlString)
        {
            StringBuilder sIn = new StringBuilder();
            foreach (object e in inArgs)
            {
                if (!typeof(ICollection).IsAssignableFrom(e.GetType()))
                {
                    object v = FormatParameters(partialSqlString, e);
                    sIn.AppendFormat("{0}{1}", sIn.Length > 0 ? "," : "", this.CreateParam(v));
                }
                else
                {
                    ICollection listArgs = e as ICollection;
                    foreach (object el in listArgs)
                    {
                        object v = FormatParameters(partialSqlString, el);
                        sIn.AppendFormat("{0}{1}", sIn.Length > 0 ? "," : "", this.CreateParam(v));
                    }
                }
            }

            if (sIn.Length == 0)
            {
                sIn.AppendFormat("select 1 /*poco_dual*/ where 1 = 0");
            }
            return sIn;
        }

        private static object FormatParameters(object partialSqlString, object e)
        {
            if (partialSqlString is EnumMemberAccess && ((EnumMemberAccess)partialSqlString).PocoColumn.ColumnType == typeof(string))
            {
                e = e.ToString();
            }
            return e;
        }

        protected virtual List<Object> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<object> list = new List<Object>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                if (original[i].NodeType == ExpressionType.NewArrayInit ||
                 original[i].NodeType == ExpressionType.NewArrayBounds)
                {

                    list.AddRange(this.VisitNewArrayFromExpressionList(original[i] as NewArrayExpression));
                }
                else
                    list.Add(this.Visit(original[i]));

            }
            return list;
        }

        protected virtual List<Object> VisitConstantList(ReadOnlyCollection<Expression> original)
        {
            List<object> list = new List<Object>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                list.Add(original[i].GetConstantValue<object>());
            }
            return list;
        }

        protected virtual object VisitNewArray(NewArrayExpression na)
        {

            List<object> exprs = this.VisitExpressionList(na.Expressions);
            StringBuilder r = new StringBuilder();
            foreach (object e in exprs)
            {
                r.Append(r.Length > 0 ? "," + e : e);
            }

            return r.ToString();
        }

        protected virtual List<Object> VisitNewArrayFromExpressionList(NewArrayExpression na)
        {

            List<object> exprs = this.VisitExpressionList(na.Expressions);
            return exprs;
        }


        protected virtual string BindOperant(ExpressionType e)
        {

            switch (e)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "MOD";
                case ExpressionType.Coalesce:
                    return "COALESCE";
                default:
                    return e.ToString();
            }
        }

        protected virtual string GetQuotedColumnName(string memberName)
        {
            PocoColumn fd = this._pocoData.Columns.Values.FirstOrDefault(x => x.MemberInfoData.Name == memberName);
            string fn = fd != null ? fd.ColumnName : memberName;
            return this._databaseType.EscapeSqlIdentifier(fn);
        }

        protected string RemoveQuoteFromAlias(string exp)
        {

            if ((exp.StartsWith("\"") || exp.StartsWith("`") || exp.StartsWith("'"))
                && (exp.EndsWith("\"") || exp.EndsWith("`") || exp.EndsWith("'")))
            {
                exp = exp.Remove(0, 1);
                exp = exp.Remove(exp.Length - 1, 1);
            }
            return exp;
        }

        protected object GetTrueExpression()
        {
            return new PartialSqlString(string.Format("({0}={1})", this.GetQuotedTrueValue(), this.GetQuotedTrueValue()));
        }

        protected object GetFalseExpression()
        {
            return new PartialSqlString(string.Format("({0}={1})", this.GetQuotedTrueValue(), this.GetQuotedFalseValue()));
        }

        protected object GetQuotedTrueValue()
        {
            return this.CreateParam(true);
        }

        protected object GetQuotedFalseValue()
        {
            return this.CreateParam(false);
        }

        private string BuildSelectExpression(List<SelectMember> fields, bool distinct)
        {
            IEnumerable<SelectMember> cols = fields ?? this._pocoData.QueryColumns.Select(x => new SelectMember{ PocoColumn = x.Value, EntityType = this._pocoData.Type, PocoColumns = new[] { x.Value }});
            return string.Format("SELECT {0}{1} \nFROM {2}",
                (distinct ? "DISTINCT " : ""),
                    string.Join(", ", cols.Select(x =>
                    {
                        if (x.SelectSql == null)
                            return (this.PrefixFieldWithTableName
                                ? this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias) + "." + this._databaseType.EscapeSqlIdentifier(x.PocoColumn.ColumnName) + " as " + this._databaseType.EscapeSqlIdentifier(x.PocoColumns.Last().MemberInfoKey)
                                : this._databaseType.EscapeSqlIdentifier(x.PocoColumn.ColumnName));
                        return x.SelectSql;
                    }).ToArray()),
                    this._databaseType.EscapeTableName(this._pocoData.TableInfo.TableName) + (this.PrefixFieldWithTableName ? " " + this._databaseType.EscapeTableName(this._pocoData.TableInfo.AutoAlias) : string.Empty));
        }

        internal List<PocoColumn> GetAllMembers()
        {
            return this._pocoData.Columns.Values.ToList();
        }

        protected virtual string ApplyPaging(string sql, IEnumerable<PocoColumn[]> columns, Dictionary<string, JoinData> joinSqlExpressions)
        {
            if (!this.Rows.HasValue || this.Rows == 0)
                return sql;

            string sqlPage;
            object[] parms = this._params.Select(x => x).ToArray();

            // Split the SQL
            PagingHelper.SQLParts parts;
            if (!PagingHelper.SplitSQL(sql, out parts)) throw new Exception("Unable to parse SQL statement for paged query");

            if (columns != null && columns.Any() && this._databaseType.UseColumnAliases())
            {
                parts.sqlColumns = string.Join(", ", columns.Select(x => this._databaseType.EscapeSqlIdentifier(x.Last().MemberInfoKey)).ToArray());
            }

            sqlPage = this._databaseType.BuildPageQuery(this.Skip ?? 0, this.Rows ?? 0, parts, ref parms);

            this._params.Clear();
            this._params.AddRange(parms);

            return sqlPage;
        }

        private string BuildInStatement(Expression m, object quotedColName)
        {
            string statement;
            UnaryExpression member = Expression.Convert(m, typeof(object));
            Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(member);
            Func<object> getter = lambda.Compile();

            if (quotedColName == null)
                quotedColName = this.Visit(m);

            IEnumerable inArgs = getter() as IEnumerable;

            StringBuilder sIn = this.FlattenList(inArgs, quotedColName);

            statement = string.Format("{0} {1} ({2})", quotedColName, "IN", sIn);
            return statement;
        }

        protected virtual object VisitSqlMethodCall(MethodCallExpression m)
        {
            List<object> args = this.VisitExpressionList(m.Arguments);
            object quotedColName = args[0];
            args.RemoveAt(0);

            string statement;

            switch (m.Method.Name)
            {
                case "In":
                    statement = this.BuildInStatement(m.Arguments[1], quotedColName);
                    break;
                case "Desc":
                    statement = string.Format("{0} DESC", quotedColName);
                    break;
                case "As":
                    statement = string.Format("{0} As {1}", quotedColName,
                        this._databaseType.EscapeSqlIdentifier(this.RemoveQuoteFromAlias(args[0].ToString())));
                    break;
                case "Sum":
                case "Count":
                case "Min":
                case "Max":
                case "Avg":
                    statement = string.Format("{0}({1}{2})",
                                         m.Method.Name.ToUpper(),
                                         quotedColName,
                                         args.Count == 1 ? string.Format(",{0}", args[0]) : "");
                    break;
                default:
                    throw new NotSupportedException();
            }

            return new PartialSqlString(statement);
        }

        protected virtual object VisitColumnAccessMethod(MethodCallExpression m)
        {
            PartialSqlString expression = (PartialSqlString)this.Visit(m.Object);

            if (this._projection && expression is MemberAccessString)
                return expression;

            string statement;
            List<object> args = this.VisitExpressionList(m.Arguments);

            switch (m.Method.Name)
            {
                case "ToUpper":
                    statement = string.Format("upper({0})", expression);
                    break;
                case "ToLower":
                    statement = string.Format("lower({0})", expression);
                    break;
                case "StartsWith":
                    statement = string.Format("upper({0}) like {1}", expression, this.CreateParam(args[0].ToString().ToUpper() + "%"));
                    break;
                case "EndsWith":
                    statement = string.Format("upper({0}) like {1}", expression, this.CreateParam("%" + args[0].ToString().ToUpper()));
                    break;
                case "Contains":
                    statement = string.Format("upper({0}) like {1}", expression, this.CreateParam("%" + args[0].ToString().ToUpper() + "%"));
                    break;
                case "Substring":
                    int startIndex = Int32.Parse(args[0].ToString()) + 1;
                    int length = (args.Count > 1) ? Int32.Parse(args[1].ToString()) : -1;
                    statement = this.SubstringStatement(expression, startIndex, length);
                    break;
                case "Equals":
                    statement = string.Format("({0} = {1})", expression, this.CreateParam(args[0]));
                    break;
                case "ToString":
                    statement = string.Empty;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return new PartialSqlString(statement);
        }

        // Easy to override
        protected virtual string SubstringStatement(PartialSqlString columnName, int startIndex, int length)
        {
            if (length >= 0)
                return string.Format("substring({0},{1},{2})", columnName, this.CreateParam(startIndex), this.CreateParam(length));
            else
                return string.Format("substring({0},{1},8000)", columnName, this.CreateParam(startIndex));
        }

    }

    public class PartialSqlString
    {
        public PartialSqlString(string text)
        {
            this.Text = text;
        }
        public string Text { get; set; }
        public override string ToString()
        {
            return this.Text;
        }
    }

    public class MemberAccessString : PartialSqlString
    {
        public MemberAccessString(PocoColumn pocoColumn, PocoColumn[] pocoColumns, string text, Type type)
            : base(text)
        {
            this.PocoColumn = pocoColumn;
            this.PocoColumns = pocoColumns;
            this.Type = type;
        }

        public PocoColumn PocoColumn { get; private set; }
        public PocoColumn[] PocoColumns { get; private set; }
        public Type Type { get; set; }
    }

    public class NullableMemberAccess : MemberAccessString
    {
        public NullableMemberAccess(PocoColumn pocoColumn, PocoColumn[] pocoColumns, string text, Type type)
            : base(pocoColumn, pocoColumns, text, type)
        {
        }
    }

    public class EnumMemberAccess : MemberAccessString
    {
        public EnumMemberAccess(PocoColumn pocoColumn, PocoColumn[] pocoColumns, string text, Type type)
            : base(pocoColumn, pocoColumns, text, type)
        {
        }
    }

    public static class LinqExtensions
    {
        /// <summary>
        /// Gets the constant value.
        /// </summary>
        /// <param retval="exp">The exp.</param>
        /// <returns>The get constant value.</returns>
        public static T GetConstantValue<T>(this Expression exp)
        {
            T result = default(T);
            if (exp is ConstantExpression)
            {
                ConstantExpression c = (ConstantExpression)exp;

                result = (T)c.Value;
            }

            return result;
        }
    }

}