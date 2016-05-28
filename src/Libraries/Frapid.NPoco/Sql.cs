using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frapid.NPoco
{
    public class Sql
    {
        public Sql()
        {
        }

        public Sql(string sql, params object[] args)
        {
            this._sql = sql;
            this._args = args;
        }

        public Sql(bool isBuilt, string sql, params object[] args)
        {
            this._sql = sql;
            this._args = args;
            if (isBuilt)
            {
                this._sqlFinal = this._sql;
                this._argsFinal = this._args;
            }
        }

        public static Sql Builder => new Sql();

        string _sql;
        object[] _args;
        Sql _rhs;
        string _sqlFinal;
        object[] _argsFinal;

        private void Build()
        {
            // already built?
            if (this._sqlFinal != null)
                return;

            // Build it
            StringBuilder sb = new StringBuilder();
            List<object> args = new List<object>();
            this.Build(sb, args, null);
            this._sqlFinal = sb.ToString();
            this._argsFinal = args.ToArray();
        }

        public string SQL
        {
            get
            {
                this.Build();
                return this._sqlFinal;
            }
        }

        public object[] Arguments
        {
            get
            {
                this.Build();
                return this._argsFinal;
            }
        }

        public Sql Append(Sql sql)
        {
            if (this._sqlFinal != null)
                this._sqlFinal = null;

            if (this._rhs != null)
            {
                this._rhs.Append(sql);
            }
            else if (this._sql != null)
            {
                this._rhs = sql;
            }
            else
            {
                this._sql = sql._sql;
                this._args = sql._args;
                this._rhs = sql._rhs;
            }

            return this;
        }

        public Sql Append(string sql, params object[] args)
        {
            return this.Append(new Sql(sql, args));
        }

        static bool Is(Sql sql, string sqltype)
        {
            return sql != null && sql._sql != null && sql._sql.StartsWith(sqltype, StringComparison.OrdinalIgnoreCase);
        }

        private void Build(StringBuilder sb, List<object> args, Sql lhs)
        {
            if (!String.IsNullOrEmpty(this._sql))
            {
                // Add SQL to the string
                if (sb.Length > 0)
                {
                    sb.Append("\n");
                }

                string sql = ParameterHelper.ProcessParams(this._sql, this._args, args);

                if (Is(lhs, "WHERE ") && Is(this, "WHERE "))
                    sql = "AND " + sql.Substring(6);
                if (Is(lhs, "ORDER BY ") && Is(this, "ORDER BY "))
                    sql = ", " + sql.Substring(9);

                sb.Append(sql);
            }

            // Now do rhs
            if (this._rhs != null)
                this._rhs.Build(sb, args, this);
        }

        public Sql Where(string sql, params object[] args)
        {
            return this.Append(new Sql("WHERE (" + sql + ")", args));
        }

        public Sql OrderBy(params object[] columns)
        {
            return this.Append(new Sql("ORDER BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        public Sql Select(params object[] columns)
        {
            return this.Append(new Sql("SELECT " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        public Sql From(params object[] tables)
        {
            return this.Append(new Sql("FROM " + String.Join(", ", (from x in tables select x.ToString()).ToArray())));
        }

        public Sql GroupBy(params object[] columns)
        {
            return this.Append(new Sql("GROUP BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        private SqlJoinClause Join(string JoinType, string table)
        {
            return new SqlJoinClause(this.Append(new Sql(JoinType + table)));
        }

        public SqlJoinClause InnerJoin(string table) { return this.Join("INNER JOIN ", table); }
        public SqlJoinClause LeftJoin(string table) { return this.Join("LEFT JOIN ", table); }

        public class SqlJoinClause
        {
            private readonly Sql _sql;

            public SqlJoinClause(Sql sql)
            {
                this._sql = sql;
            }

            public Sql On(string onClause, params object[] args)
            {
                return this._sql.Append("ON " + onClause, args);
            }
        }

        public static implicit operator Sql(SqlBuilder.Template template)
        {
            return new Sql(true, template.RawSql, template.Parameters);
        }
    }
}