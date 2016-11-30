using System.Linq;

namespace Frapid.Mapper.Helpers
{
    public sealed class UnderscoreCaseConverter
    {
        public string Convert(string name)
        {
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        }
    }
}