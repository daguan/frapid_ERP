using System.IO;
using System.Web.Hosting;

namespace Frapid.Configuration
{
    public static class PathMapper
    {
        public static string PathToRootDirectory;

        public static string MapPath(string path)
        {
            if(string.IsNullOrWhiteSpace(PathToRootDirectory))
            {
                PathToRootDirectory = HostingEnvironment.MapPath("~/");
            }

            if(string.IsNullOrWhiteSpace(PathToRootDirectory))
            {
                return path;
            }

            if(path.StartsWith("~/"))
            {
                path = path.Remove(0, 2);
            }

            if(path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return Path.Combine(PathToRootDirectory, path);
        }
    }
}