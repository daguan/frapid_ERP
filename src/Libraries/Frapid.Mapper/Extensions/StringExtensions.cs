using Frapid.Mapper.Helpers;

namespace Frapid.Mapper.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string name)
        {
            var titleCaseConverter = new TitleCaseConverter();
            return titleCaseConverter.Convert(name);
        }

        public static string ToUnderscoreCase(this string name)
        {
            var underscoreCaseConverter = new UnderscoreCaseConverter();
            return underscoreCaseConverter.Convert(name);
        }

        public static string ToUnderscoreLowerCase(this string name)
        {
            return ToUnderscoreCase(name).ToLower();
        }

        public static string ToPascalCase(this string name)
        {
            var titleCaseConverter = new TitleCaseConverter();
            var pascalCaseConverter = new PascalCaseConverter(titleCaseConverter);
            return pascalCaseConverter.Convert(name);
        }
    }
}