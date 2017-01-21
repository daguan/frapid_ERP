using System.Collections.Generic;

namespace Frapid.Mapper.Helpers
{
    public sealed class UnderscoreCaseConverter
    {
        public string Convert(string name)
        {
            var letters = new List<char> { name[0] };
            int length = name.Length;

            for (int i = 1; i < length; i++)
            {
                char letter = name[i];
                bool isNumber = char.IsNumber(letter);

                if (!isNumber && char.IsUpper(letter))
                {
                    letters.Add('_');
                    letters.Add(letter);
                }
                else if (char.IsNumber(letter))
                {
                    char previous = name[i - 1];

                    if (!char.IsNumber(previous))
                    {
                        letters.Add('_');
                    }

                    letters.Add(letter);

                    if (i + 1 >= length)
                    {
                        continue;
                    };

                    char next = name[i + 1];

                    if (!char.IsNumber(next))
                    {
                        letters.Add('_');
                    }
                }
                else
                {
                    letters.Add(letter);
                }
            }

            string result = string.Concat(letters).ToLowerInvariant();
            return result;
        }
    }
}