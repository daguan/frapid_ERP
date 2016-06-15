using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Frapid.Areas.Drawing
{
    public static class BitmapHelper
    {
        public static Image Resize(this Image img, int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth,
            int dstHeight)
        {
            var bmp = new Bitmap(dstWidth, dstHeight);
            {
                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        var destRect = new Rectangle(0, 0, dstWidth, dstHeight);
                        graphics.DrawImage(img, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return bmp;
            }
        }

        public static Image ResizeProportional(this Image img, int width, int height, bool enlarge = false)
        {
            double ratio = Math.Max(img.Width / (double)width, img.Height / (double)height);
            if (ratio < 1 && !enlarge)
            {
                return img;
            }
            return img.Resize(0, 0, img.Width, img.Height, (int)Math.Round(img.Width / ratio),
                (int)Math.Round(img.Height / ratio));
        }

        public static byte[] ResizeCropExcess(string path, int dstWidth = 0, int dstHeight = 0)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            using (var img = new Bitmap(path))
            {
                if (dstWidth == 0)
                {
                    dstWidth = img.Width;
                }
                if (dstHeight == 0)
                {
                    dstHeight = img.Height;
                }

                double srcRatio = img.Width / (double)img.Height;
                double dstRatio = dstWidth / (double)dstHeight;
                int srcX, srcY, cropWidth, cropHeight;

                if (srcRatio < dstRatio) // trim top and bottom
                {
                    cropHeight = dstHeight * img.Width / dstWidth;
                    srcY = (img.Height - cropHeight) / 2;
                    cropWidth = img.Width;
                    srcX = 0;
                }
                else // trim left and right
                {
                    cropWidth = dstWidth * img.Height / dstHeight;
                    srcX = (img.Width - cropWidth) / 2;
                    cropHeight = img.Height;
                    srcY = 0;
                }

                using (var stream = new MemoryStream())
                {
                    var image = Resize(img, srcX, srcY, cropWidth, cropHeight, dstWidth, dstHeight);
                    image.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }
}