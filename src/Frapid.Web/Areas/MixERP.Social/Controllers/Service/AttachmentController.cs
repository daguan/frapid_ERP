using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.Areas.Conventions.Attachments;
using Frapid.Areas.CSRF;
using Frapid.Areas.Drawing;
using Frapid.Configuration;
using Frapid.WebsiteBuilder;
using MixERP.Social.DTO;
using MixERP.Social.Helpers;
using Serilog;

namespace MixERP.Social.Controllers.Service
{
    [AntiForgery]
    public class AttachmentController : FrapidController
    {
        [Route("dashboard/social/attachment")]
        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                var files = new List<UploadedFile>();
                string uploadDirectory = this.GetUploadDirectory(this.Tenant);

                for (int i = 0; i < this.Request.Files.Count; i++)
                {
                    var uploaded = this.Upload(uploadDirectory, this.Request.Files[i]);
                    files.Add(uploaded);
                }

                return this.Ok(files);
            }
            catch (Exception ex)
            {
                return this.Failed(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public string GetUploadDirectory(string tenant)
        {
            string path = $"/Tenants/{tenant}/Areas/MixERP.Social/uploads/";
            path = PathMapper.MapPath(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public UploadedFile Upload(string uploadDirectory, HttpPostedFileBase file)
        {
            if (file == null)
            {
                throw new UploadException("No file was uploaded.");
            }

            string fileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(fileName);
            string savedFile = Guid.NewGuid() + extension;

            if (string.IsNullOrEmpty(fileName))
            {
                Log.Information("Could not upload resource because the posted attachment file name is null or invalid.");
                throw new UploadException(string.Empty);
            }

            var allowed = FrapidConfig.GetAllowedUploadExtensions(this.Tenant);

            if (!allowed.Contains(extension))
            {
                Log.Warning("Could not upload attachment resource because the uploaded file {file} has invalid extension.", fileName);
                throw new UploadException("Invalid extension.");
            }

            var stream = file.InputStream;
            string path = Path.Combine(uploadDirectory, savedFile);

            using (var fileStream = System.IO.File.Create(path))
            {
                stream.CopyTo(fileStream);
            }

            return new UploadedFile
            {
                OriginalFileName = fileName,
                FileName = savedFile
            };
        }

        [Route("dashboard/social/attachment/{fileName}")]
        [Route("dashboard/social/attachment/{fileName}/{width}")]
        [Route("dashboard/social/attachment/{fileName}/{width}/{height}")]
        [FileOutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(string fileName, int width = 0, int height = 0)
        {
            string path = this.GetUploadDirectory(this.Tenant);
            path = Path.Combine(path, fileName);

            if (!System.IO.File.Exists(path))
            {
                return this.HttpNotFound();
            }
            string mimeType = MimeMapping.GetMimeMapping(path);
            return this.File(path, mimeType);
        }

    }
}