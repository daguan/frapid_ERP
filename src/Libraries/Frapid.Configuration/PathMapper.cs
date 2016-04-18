using System.IO;
using System.Web;

namespace Frapid.Configuration
{
    public static class PathMapper
    {
        public static string PathToRootDirectory;

        public static string MapPath(string path)
        {
            var context = HttpContext.Current;

            if (context != null)
            {
                return context.Server.MapPath(path);
            }

            if (path.StartsWith("~/"))
            {
                path = path.Remove(0, 2);
            }

            if (path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return Path.Combine(PathToRootDirectory, path);
        }
    }
}