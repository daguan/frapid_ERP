using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.Framework;
using Serilog;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeUploader
    {
        public ThemeUploader(HttpPostedFileBase postedFile)
        {
            this.PostedFile = postedFile;
            this.ThemeInfo = new ThemeInfo();
            this.SetUploadPaths();
        }

        public ThemeUploader(Uri downloadUrl)
        {
            this.DownloadUrl = downloadUrl;
            this.ThemeInfo = new ThemeInfo();
            this.SetUploadPaths();
        }

        public Uri DownloadUrl { get; }
        public string FileName { get; private set; }
        public string ArchivePath { get; private set; }
        public string ExtractedDirectory { get; private set; }
        public HttpPostedFileBase PostedFile { get; }
        public ThemeInfo ThemeInfo { get; private set; }

        private void SetUploadPaths()
        {
            this.FileName = Guid.NewGuid().ToString();
            this.ArchivePath = Path.Combine(Path.Combine(this.GetUploadDirectory(), this.FileName + ".zip"));
        }

        private void Download()
        {
            if (this.DownloadUrl == null)
            {
                throw new ThemeInstallException("Could not download theme because supplied URL is invalid or missing.");
            }

            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(this.DownloadUrl, this.ArchivePath);
                }
                catch (WebException ex)
                {
                    throw new ThemeInstallException(ex.Message, ex);
                }
            }
        }

        private string GetUploadDirectory()
        {
            string catalog = DbConvention.GetCatalog();
            string uploadDirectory = HostingEnvironment.MapPath($"~/Catalogs/{catalog}/Temp/");

            if (uploadDirectory == null || !Directory.Exists(uploadDirectory))
            {
                Log.Warning("Could not upload theme because the temporary directory {uploadDirectory} does not exist.",
                    uploadDirectory);
                throw new ThemeUploadException(
                    "Could not upload your theme. Check application logs for more information.");
            }

            return uploadDirectory;
            ;
        }

        private void Upload()
        {
            string extension = Path.GetExtension(this.PostedFile.FileName);

            if (extension == null || extension.ToLower() != ".zip")
            {
                Log.Warning("Could not upload theme because the uploaded file {file} has invalid extension.",
                    this.PostedFile.FileName);
                throw new ThemeUploadException("Could not upload theme because the uploaded file has invalid extension.");
            }

            var stream = this.PostedFile.InputStream;

            using (var fileStream = File.Create(this.ArchivePath))
            {
                stream.CopyTo(fileStream);
            }
        }

        private void ExtractTheme()
        {
            this.ExtractedDirectory = this.ArchivePath.Replace(".zip", "");

            try
            {
                ZipFile.ExtractToDirectory(this.ArchivePath, this.ExtractedDirectory);
            }
            catch (InvalidDataException ex)
            {
                throw new ThemeInstallException("Could not install theme because the supplied file was not a valid ZIP archive.", ex);
            }
        }

        private bool Validate()
        {
            string configFile = Path.Combine(this.ExtractedDirectory, "Theme.config");

            if (!File.Exists(configFile))
            {
                return false;
            }

            var info = ThemeInfoParser.Parse(configFile);

            if (info == null || !info.IsValid)
            {
                return false;
            }

            this.ThemeInfo = info;
            return true;
        }

        public void CopyTheme()
        {
            string source = this.ExtractedDirectory;
            string catalog = DbConvention.GetCatalog();
            string destination =
                HostingEnvironment.MapPath(
                    $"~/Catalogs/{catalog}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeInfo.ThemeName}");

            if (destination == null)
            {
                Log.Warning("Could not copy theme because the destination directory could not be located.");
                throw new ThemeInstallException("Could not install theme. Check application logs for more information.");
            }

            if (Directory.Exists(destination))
            {
                throw new ThemeInstallException("Could not install theme because it already exists.");
            }

            FileHelper.CopyDirectory(source, destination);
        }

        public ThemeInfo Install()
        {
            try
            {
                if (this.PostedFile == null)
                {
                    this.Download();
                }
                else
                {
                    this.Upload();
                }

                this.ExtractTheme();

                bool isValid = this.Validate();

                if (!isValid)
                {
                    throw new ThemeInstallException("The uploaded archive is not a valid frapid theme!");
                }

                this.CopyTheme();
                return this.ThemeInfo;
            }
            finally
            {
                if (Directory.Exists(this.ArchivePath.Replace(".zip", "")))
                {
                    Directory.Delete(this.ArchivePath.Replace(".zip", ""), true);
                }
                if (File.Exists(this.ArchivePath))
                {
                    File.Delete(this.ArchivePath);
                }
            }
        }
    }
}