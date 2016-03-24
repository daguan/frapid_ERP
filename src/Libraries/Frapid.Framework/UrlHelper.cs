using System;

namespace Frapid.Framework
{
    public static class UrlHelper
    {
        public static string CombineUrl(string domain, string path)
        {
            Uri result;

            if (Uri.TryCreate(new Uri(domain), path, out result))
            {
                return result.ToString();
            }

            return domain + "/" + path;
        }
    }
}