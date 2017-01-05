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

                if (char.IsUpper(letter))
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