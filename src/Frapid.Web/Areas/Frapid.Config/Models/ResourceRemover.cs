using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.Config.Models
{
    public sealed class ResourceRemover
    {
        public ResourceRemover(string resource)
        {
            this.Resource = resource;
        }

        public string Resource { get; }

        public void Delete()
        {
            string tenant = DbConvention.GetTenant();
            string path = $"~/Tenants/{tenant}";
            path = HostingEnvironment.MapPath(path);

            if (path == null)
            {
                throw new ResourceRemoveException("Path to the file or directory is invalid.");
            }

            path = Path.Combine(path, this.Resource);

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
                return;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
                return;
            }

            throw new ResourceRemoveException("File or directory could not be found.");
        }
    }
}