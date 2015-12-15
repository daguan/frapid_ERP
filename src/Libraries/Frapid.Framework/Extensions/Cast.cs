using System.ComponentModel;

namespace Frapid.Framework.Extensions
{
    public static class Cast
    {
        public static T To<T>(this string input)
        {
            var d = default(T);

            if (string.IsNullOrWhiteSpace(input))
            {
                return d;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input);
        }

        public static T To<T>(this string input, T or)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return or;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input);
        }

        public static T To<T>(this object input)
        {
            var d = default(T);

            if (input == null)
            {
                return d;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input.ToString());
        }

        public static T To<T>(this object input, T or)
        {
            if (input == null)
            {
                return or;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input.ToString());
        }
    }
}