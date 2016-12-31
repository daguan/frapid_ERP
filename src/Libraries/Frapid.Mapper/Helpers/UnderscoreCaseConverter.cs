using System.Linq;

namespace Frapid.Mapper.Helpers
{
    public sealed class UnderscoreCaseConverter
    {
        public string Convert(string name)
        {
            string result = string.Concat(name.Select((x, i) => i > 0 && (char.IsUpper(x) || char.IsNumber(x)) ? "_" + x.ToString() : x.ToString())).ToLowerInvariant();

            return result;
        }
    }
}