using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using Frapid.Areas.Drawing;

namespace MixERP.Social.Helpers
{
    public static class AvatarHelper
    {
        public static string GetAvatarImagePath(string tenant, string userId)
        {
            string path = $"~/Tenants/{tenant}/Areas/MixERP.Social/avatars/";
            path = HostingEnvironment.MapPath(path);

            if (path == null || !Directory.Exists(path))
            {
                return string.Empty;
            }

            var extensions = new[] {".png", ".jpg", ".jpeg", ".gif"};
            var directory = new DirectoryInfo(path);

            var files = directory.GetFiles();

            var candidate = files.FirstOrDefault(
                f => Path.GetFileNameWithoutExtension(f.Name) == userId
                     && extensions.Contains(f.Extension.ToLower())
                );

            return candidate?.FullName ?? string.Empty;
        }


        public static byte[] FromFile(string path)
        {
            return BitmapHelper.ResizeCropExcess(path);
        }

        public static byte[] FromName(string name)
        {
            string[] colors =
            {
                "#24B7B5", "#269C26", "#78B639", "#AECA1C", "#B624FF", "#7B1DFF", "#2268F1", "#3DAFE6",
                "#E93726", "#AF2444", "#DE2487", "#F581D5", "#94724B", "#E5CB10", "#F2B02C", "#FB7E26",
                "#958963", "#826E94", "#798997", "#83997B"
            };
            string text = ExtractInitialsFromName(name);
            int colorIndex = name.Select(i => (int) i).Sum()%20;

            using (var bitmap = new Bitmap(500, 500))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    using (var font = new Font("Arial", 150, FontStyle.Regular, GraphicsUnit.Point))
                    {
                        var rectangle = new Rectangle(0, 0, 500, 500);

                        using (var b = new SolidBrush(ColorTranslator.FromHtml(colors[colorIndex])))
                        {
                            graphics.FillRectangle(b, rectangle);
                            graphics.DrawString(text, font, Brushes.White, rectangle,
                                new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                        }
                    }
                }

                using (var memStream = new MemoryStream())
                {
                    bitmap.Save(memStream, ImageFormat.Png);
                    return memStream.GetBuffer();
                }
            }
        }

        /// <param name="name">The full name of a person.</param>
        /// <returns>One to two uppercase initials, without punctuation.</returns>
        private static string ExtractInitialsFromName(string name)
        {
            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(name, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            // Extract up to 2 initials from the remaining cleaned name.
            initials =
                Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2")
                    .Trim();

            if (initials.Length > 2)
            {
                // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            return initials.ToUpperInvariant();
        }
    }
}