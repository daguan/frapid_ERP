using System.Linq;
using System.Text.RegularExpressions;

namespace Frapid.NPoco
{
    public static class StringExtensions
    {
        public static string BreakUpCamelCase(this string s)
        {
            string[] patterns = new[]
            {
                "([a-z])([A-Z])",
                "([0-9])([a-zA-Z])",
                "([a-zA-Z])([0-9])"
            };
            string output = patterns.Aggregate(s, (current, pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));
            return output;
        }
    }
}
