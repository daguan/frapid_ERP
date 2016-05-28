namespace Frapid.Framework
{
    public static class UrlHelper
    {
        private static readonly UrlCombiner Combiner;

        static UrlHelper()
        {
            Combiner = new UrlCombiner();
        }

        public static string CombineUrl(string domain, string path)
        {
            return Combiner.Combine(domain, path);
        }
    }
}