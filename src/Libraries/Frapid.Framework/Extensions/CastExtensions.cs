namespace Frapid.Framework.Extensions
{
    public static class CastExtensions
    {
        public static T To<T>(this string input)
        {
            var castable = new Castable();
            return castable.To<T>(input);
        }

        public static T To<T>(this string input, T or)
        {
            var castable = new Castable();
            return castable.To(input, or);
        }

        public static T To<T>(this object input)
        {
            var castable = new Castable();
            return castable.To<T>(input);
        }

        public static T To<T>(this object input, T or)
        {
            var castable = new Castable();
            return castable.To(input, or);
        }
    }
}