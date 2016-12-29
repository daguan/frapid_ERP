using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.Areas.CSRF;
using Frapid.Areas.Drawing;
using Frapid.Configuration;
using Frapid.WebsiteBuilder;
using MixERP.Social.Helpers;
using Serilog;

namespace MixERP.Social.Controllers.Service
{
    [AntiForgery]
    public class AvatarController : FrapidController
    {
        [Route("dashboard/social/avatar")]
        [HttpPost]
        public async Task<ActionResult> PostAsync()
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);
            if (this.Request.Files.Count > 1)
            {
                return this.Failed("Only single file may be uploaded.", HttpStatusCode.BadRequest);
            }

            var file = this.Request.Files[0];
            if (file == null)
            {
                return this.Failed("No file was uploaded.", HttpStatusCode.BadRequest);
            }


            int me = meta.UserId;

            string path = $"/Tenants/{this.Tenant}/Areas/MixERP.Social/avatars";
            path = PathMapper.MapPath(path);

            if (path == null || !Directory.Exists(path))
            {
                Log.Warning("Could not upload resource because the avatar directory {directory} does not exist.", path);
                return this.Failed(string.Empty, HttpStatusCode.InternalServerError);
            }

            string fileName = Path.GetFileName(file.FileName);

            if (fileName == null)
            {
                Log.Warning("Could not upload resource because the posted avatar file name is null or invalid.");
                return this.Failed(string.Empty, HttpStatusCode.InternalServerError);
            }

            var allowed = FrapidConfig.GetAllowedUploadExtensions(this.Tenant);
            string extension = Path.GetExtension(fileName);
            if (!allowed.Contains(extension))
            {
                Log.Warning("Could not upload avatar resource because the uploaded file {file} has invalid extension.",
                    fileName);
                return this.Failed(string.Empty, HttpStatusCode.InternalServerError);
            }

            var dir = new DirectoryInfo(path);
            foreach (
                var f in dir.GetFiles().Where(f => Path.GetFileNameWithoutExtension(f.Name).Equals(me.ToString())))
            {
                f.Delete();
            }

            var stream = file.InputStream;
            path = Path.Combine(path, me + extension);

            using (var fileStream = System.IO.File.Create(path))
            {
                stream.CopyTo(fileStream);
            }

            return this.Ok();
        }


        [Route("dashboard/social/avatar/{userId}/{name}")]
        [Route("dashboard/social/avatar/{userId}/{name}/{width}")]
        [Route("dashboard/social/avatar/{userId}/{name}/{width}/{height}")]
        [FileOutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int userId, string name, int width = 0, int height = 0)
        {
            string imagePath = AvatarHelper.GetAvatarImagePath(this.Tenant,
                userId.ToString(CultureInfo.InvariantCulture));

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return this.File(AvatarHelper.FromName(name), "image/png");
            }

            string mimeType = MimeMapping.GetMimeMapping(imagePath);


            return this.File(BitmapHelper.ResizeCropExcess(imagePath, width, height), mimeType);
        }
    }
}