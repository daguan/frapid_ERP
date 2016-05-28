using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Frapid.Configuration;
using Serilog;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ResourceUploader
    {
        public ResourceUploader(HttpPostedFileBase postedFile, string themeName, string container)
        {
            this.PostedFile = postedFile;
            this.ThemeName = themeName;
            this.Container = container;
        }

        public HttpPostedFileBase PostedFile { get; }
        public string ThemeName { get; }
        public string Container { get; }

        public void Upload()
        {
            if (string.IsNullOrWhiteSpace(this.ThemeName))
            {
                throw new ArgumentNullException(nameof(this.ThemeName));
            }

            string tenant = TenantConvention.GetTenant();
            string path = $"~/Tenants/{tenant}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeName}";
            path = HostingEnvironment.MapPath(path);

            if (path == null || !Directory.Exists(path))
            {
                Log.Warning("Could not upload resource because the directory {directory} does not exist.", path);
                throw new ResourceUploadException("Invalid path. Check the log for more details.");
            }

            path = Path.Combine(path, this.Container);

            if (!Directory.Exists(path))
            {
                Log.Warning("Could not upload resource because the directory {directory} does not exist.", path);
                throw new ResourceUploadException("Invalid path. Check application logs for more information.");
            }


            string fileName = Path.GetFileName(this.PostedFile.FileName);

            if (fileName == null)
            {
                Log.Warning("Could not upload resource because the posted file name is null or invalid.");
                throw new ResourceUploadException(
                    "Could not upload resource because the posted file name is null or invalid.");
            }

            var allowed = FrapidConfig.GetAllowedUploadExtensions(TenantConvention.GetTenant());
            string extension = Path.GetExtension(fileName);

            if (!allowed.Contains(extension))
            {
                Log.Warning("Could not upload resource because the uploaded file {file} has invalid extension.",
                    fileName);
                throw new ResourceUploadException(
                    "Could not upload resource because the uploaded file has invalid extension.");
            }

            var stream = this.PostedFile.InputStream;
            path = Path.Combine(Path.Combine(path, fileName));

            using (var fileStream = File.Create(path))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}