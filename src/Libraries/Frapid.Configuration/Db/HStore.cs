using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Frapid.Configuration.Db
{
    public class HStore : Dictionary<string, string>
    {
        private static readonly Regex QuotedString = new Regex(@"""[^""\\]*(?:\\.[^""\\]*)*""", RegexOptions.Compiled);
        private static readonly Regex UnquotedString = new Regex(@"(?:\\.|[^\s,])[^\s=,\\]*(?:\\.[^\s=,\\]*|=[^,>])*", RegexOptions.Compiled);
        private static readonly Regex HstoreKeyPairMatch = new Regex(string.Format(@"({0}|{1})\s*=>\s*({0}|{1})", QuotedString, UnquotedString), RegexOptions.Compiled);
        private static readonly Regex KeyReg = new Regex(@"^""(.*)""$", RegexOptions.Compiled);
        private static readonly Regex KeyReg2 = new Regex(@"\\(.)", RegexOptions.Compiled);
        private static readonly Regex EscapeRegex = new Regex(@"[=\s,>]", RegexOptions.Compiled);
        private static readonly Regex EscapeRegex2 = new Regex(@"([""\\])", RegexOptions.Compiled);

        public static Dictionary<Type, Func<object, string>> Formats = new Dictionary<Type, Func<object, string>>();

        public void Add(string key, object value)
        {
            Func<object, string> format;
            if (value != null && Formats.TryGetValue(value.GetType(), out format))
            {
                base.Add(key, format(value));
            }
            else
            {
                base.Add(key, value?.ToString());
            }
        }

        public string ToSqlString()
        {
            return Create(this);
        }

        public static string Create(Dictionary<string, string> value)
        {
            var keypairs = value.Select(keypair => Escape(keypair.Key) + "=>" + Escape(keypair.Value));
            return string.Join(",", keypairs);
        }

        public static HStore Create(string value)
        {
            var matches = HstoreKeyPairMatch.Matches(value);
            var hstore = new HStore();

            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value;
                string val = match.Groups[2].Value;

                key = KeyReg2.Replace(KeyReg.Replace(key, "$1"), "$1");
                val = val.ToUpper() == "NULL" ? null : KeyReg2.Replace(KeyReg.Replace(val, "$1"), "$1");

                hstore.Add(key, val);
            }

            return hstore;
        }

        public static string Escape(string value)
        {
            return value == null ? "NULL"
                : EscapeRegex.IsMatch(value) ? $"\"{EscapeRegex2.Replace(value, "\\\\$1")}\""
                    : value == "" ? "\"\""
                        : EscapeRegex2.Replace(value, "\\\\$1");
        }
    }
}