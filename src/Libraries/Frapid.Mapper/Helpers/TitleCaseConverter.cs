using System.Collections.Generic;
using System.Linq;

namespace Frapid.Mapper.Helpers
{
    public sealed class TitleCaseConverter
    {
        public string Convert(string name)
        {
            return new string(this.ToTitleCase(name).ToArray());
        }

        private IEnumerable<char> ToTitleCase(string name)
        {
            bool newWord = true;

            foreach (char c in name)
            {
                if (newWord)
                {
                    yield return char.ToUpper(c);
                    newWord = false;
                }
                else
                {
                    yield return char.ToLower(c);
                }

                if (c == ' ')
                {
                    newWord = true;
                }
            }
        }
    }
}