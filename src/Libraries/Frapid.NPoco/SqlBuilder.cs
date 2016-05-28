using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Frapid.NPoco
{
    public class SqlBuilder
    {
        Dictionary<string, Clauses> data = new Dictionary<string, Clauses>();
        int seq;

        class Clause
        {
            public string Sql { get; set; }
            public string ResolvedSql { get; set; }
            public List<object> Parameters { get; set; }
        }

        class Clauses : List<Clause>
        {
            string joiner;
            string prefix;
            string postfix;

            public Clauses(string joiner, string prefix, string postfix)
            {
                this.joiner = joiner;
                this.prefix = prefix;
                this.postfix = postfix;
            }

            public string ResolveClauses(List<object> finalParams)
            {
                foreach (Clause item in this)
                {
                    item.ResolvedSql = ParameterHelper.ProcessParams(item.Sql, item.Parameters.ToArray(), finalParams);
                }
                return this.prefix + string.Join(this.joiner, this.Select(c => c.ResolvedSql).ToArray()) + this.postfix;
            }
        }

        public class Template
        {
            public bool TokenReplacementRequired { get; set; }

            readonly string sql;
            readonly SqlBuilder builder;
            private List<object> finalParams = new List<object>();
            int dataSeq;

            public Template(SqlBuilder builder, string sql, params object[] parameters)
            {
                this.sql = ParameterHelper.ProcessParams(sql, parameters, this.finalParams);
                this.builder = builder;
            }

            static Regex regex = new Regex(@"(\/\*\*.+\*\*\/)", RegexOptions.Compiled | RegexOptions.Multiline);

            void ResolveSql()
            {
                if (this.dataSeq != this.builder.seq)
                {
                    this.rawSql = this.sql;
                    foreach (KeyValuePair<string, Clauses> pair in this.builder.data)
                    {
                        this.rawSql = this.rawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(this.finalParams));
                    }

                    this.ReplaceDefaults();

                    this.dataSeq = this.builder.seq;
                }

                if (this.builder.seq == 0)
                {
                    this.rawSql = this.sql;
                    this.ReplaceDefaults();
                }
            }

            private void ReplaceDefaults()
            {
                if (this.TokenReplacementRequired)
                {
                    foreach (KeyValuePair<string, string> pair in this.builder.defaultsIfEmpty)
                    {
                        string fullToken = GetFullTokenRegexPattern(pair.Key);
                        if (Regex.IsMatch(this.rawSql, fullToken))
                        {
                            throw new Exception(string.Format("Token '{0}' not used. All tokens must be replaced if TokenReplacementRequired switched on.",fullToken));
                        }
                    }
                }

                this.rawSql = regex.Replace(this.rawSql, x =>
                {
                    string token = x.Groups[1].Value;
                    bool found = false;

                    foreach (KeyValuePair<string, string> pair in this.builder.defaultsIfEmpty)
                    {
                        string fullToken = GetFullTokenRegexPattern(pair.Key);
                        if (Regex.IsMatch(token, fullToken))
                        {
                            if (pair.Value != null)
                            {
                                token = Regex.Replace(token, fullToken, " " + pair.Value + " ");
                            }
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        token = string.Empty;
                    }

                    return token;
                });
            }

            private static string GetFullTokenRegexPattern(string key)
            {
                return @"/\*\*" + key + @"\*\*/";
            }

            string rawSql;

            public string RawSql { get { this.ResolveSql(); return this.rawSql; } }
            public object[] Parameters { get { this.ResolveSql(); return this.finalParams.ToArray(); } }
        }

        /// <summary>
        /// Initialises the SqlBuilder
        /// </summary>
        public SqlBuilder()
        {
        }

        /// <summary>
        /// Initialises the SqlBuilder with default replacement overrides
        /// </summary>
        /// <param name="defaultOverrides">A dictionary of token overrides. A value null means the token will not be replaced.</param>
        /// <example>
        /// { "where", "1=1" }
        /// { "where(name)", "1!=1" }
        /// </example>
        public SqlBuilder(Dictionary<string, string> defaultOverrides)
        {
            this.defaultsIfEmpty.InsertRange(0, defaultOverrides.Select(x => new KeyValuePair<string, string>(Regex.Escape(x.Key), x.Value)));
        }

        public Template AddTemplate(string sql, params object[] parameters)
        {
            return new Template(this, sql, parameters);
        }

        void AddClause(string name, string sql, object[] parameters, string joiner, string prefix, string postfix)
        {
            Clauses clauses;
            if (!this.data.TryGetValue(name, out clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                this.data[name] = clauses;
            }
            clauses.Add(new Clause { Sql = sql, Parameters = new List<object>(parameters) });
            this.seq++;
        }

        readonly List<KeyValuePair<string, string>> defaultsIfEmpty = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(@"where\([\w]+\)", "1=1"),
            new KeyValuePair<string, string>("where", "1=1"),
            new KeyValuePair<string, string>("select", "1")
        };
        
        /// <summary>
        /// Replaces the Select columns. Uses /**select**/
        /// </summary>
        public SqlBuilder Select(params string[] columns)
        {
            this.AddClause("select", string.Join(", ", columns), new object[] { }, ", ", "", "");
            return this;
        }

        /// <summary>
        /// Adds an Inner Join. Uses /**join**/
        /// </summary>
        public SqlBuilder Join(string sql, params object[] parameters)
        {
            this.AddClause("join", sql, parameters, "\nINNER JOIN ", "\nINNER JOIN ", "\n");
            return this;
        }

        /// <summary>
        /// Adds a Left Join. Uses /**leftjoin**/
        /// </summary>
        public SqlBuilder LeftJoin(string sql, params object[] parameters)
        {
            this.AddClause("leftjoin", sql, parameters, "\nLEFT JOIN ", "\nLEFT JOIN ", "\n");
            return this;
        }

        /// <summary>
        /// Adds a filter. The Where keyword still needs to be specified. Uses /**where**/
        /// </summary>
        public SqlBuilder Where(string sql, params object[] parameters)
        {
            this.AddClause("where", "( " + sql + " )", parameters, " AND ", "", "\n");
            return this;
        }

        /// <summary>
        /// Adds a named filter. The Where keyword still needs to be specified. Uses /**where(name)**/
        /// </summary>
        public SqlBuilder WhereNamed(string name, string sql, params object[] parameters)
        {
            this.AddClause("where(" + name + ")", "( " + sql + " )", parameters, " AND ", "", "\n");
            return this;
        }

        /// <summary>
        /// Adds an Order By clause. Uses /**orderby**/
        /// </summary>
        public SqlBuilder OrderBy(string sql, params object[] parameters)
        {
            this.AddClause("orderby", sql, parameters, ", ", "ORDER BY ", "\n");
            return this;
        }

        /// <summary>
        /// Adds columns in the Order By clause. Uses /**orderbycols**/
        /// </summary>
        public SqlBuilder OrderByCols(params string[] columns)
        {
            this.AddClause("orderbycols", string.Join(", ", columns), new object[] { }, ", ", ", ", "");
            return this;
        }

        /// <summary>
        /// Adds a Group By clause. Uses /**groupby**/
        /// </summary>
        public SqlBuilder GroupBy(string sql, params object[] parameters)
        {
            this.AddClause("groupby", sql, parameters, " , ", "\nGROUP BY ", "\n");
            return this;
        }

        /// <summary>
        /// Adds a Having clause. Uses /**having**/
        /// </summary>
        public SqlBuilder Having(string sql, params object[] parameters)
        {
            this.AddClause("having", sql, parameters, "\nAND ", "HAVING ", "\n");
            return this;
        }
    }
}