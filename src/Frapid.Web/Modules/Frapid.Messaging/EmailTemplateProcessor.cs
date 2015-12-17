using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Frapid.Messaging
{
    public class EmailTemplateProcessor
    {
        public EmailTemplateProcessor(string template, List<object> dictionary)
        {
            Template = template;
            Dictionary = dictionary;
        }

        public string Template { get; }
        public List<object> Dictionary { get; }

        public string Process()
        {
            var parameters = GetParameters(Template);
            string template = Template;

            foreach (var item in Dictionary)
            {
                foreach (string parameter in parameters)
                {
                    string value = GetPropertyValue(item, parameter);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        template = template.Replace("{" + parameter + "}", value);
                    }
                }
            }

            //Remove null parameters
            parameters = GetParameters(Template);

            return parameters.Aggregate(template,
                (current, parameter) => current.Replace("{" + parameter + "}", string.Empty));
        }

        private List<string> GetParameters(string template)
        {
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})", RegexOptions.IgnoreCase);
            var matches = regex.Matches(template);

            return matches.Cast<Match>().Select(m => m.Value.Replace("{", "}")).Distinct().ToList();
        }

        private static string GetPropertyValue(object obj, string propertyName)
        {
            var prop = obj?.GetType().GetProperty(propertyName);
            var value = prop?.GetValue(obj, null);
            return value?.ToString() ?? string.Empty;
        }
    }
}