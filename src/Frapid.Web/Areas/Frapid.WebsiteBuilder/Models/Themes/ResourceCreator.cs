using System.IO;
using System.Text;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ResourceCreator
    {
        public string ThemeName { get; set; }
        public string Container { get; set; }
        public string File { get; set; }
        public bool IsDirectory { get; set; }
        public string Contents { get; set; }
        public bool Rewrite { get; set; }

        public void Create()
        {
            if (string.IsNullOrWhiteSpace(this.ThemeName) || string.IsNullOrWhiteSpace(this.File))
            {
                throw new ResourceCreateException("Invalid theme or file name.");
            }

            string catalog = DbConvention.GetCatalog();
            string path = $"~/Catalogs/{catalog}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeName}";
            path = HostingEnvironment.MapPath(path);

            if (path == null || !Directory.Exists(path))
            {
                throw new ResourceCreateException(
                    "Could not create the file or directory because the theme directory was not found.");
            }

            path = Path.Combine(path, this.Container);

            if (!Directory.Exists(path))
            {
                throw new ResourceCreateException("Could not create the file or directory in an invalid directory path.");
            }

            path = Path.Combine(path, this.File);

            if (this.Rewrite.Equals(false) && System.IO.File.Exists(path))
            {
                return;
            }

            if (this.IsDirectory)
            {
                Directory.CreateDirectory(path);
                return;
            }

            System.IO.File.WriteAllText(path, this.Contents, Encoding.UTF8);
        }
    }
}