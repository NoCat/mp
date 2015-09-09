using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace mp.Utility
{
    static public class BitmapExtend
    {
        /// <summary>
        /// 截取方形图像
        /// </summary>
        public static Image Square(this Image img, int size)
        {
            int top, left, s;
            if (img.Width > img.Height)
            {
                s = img.Height;
                top = 0;
                left = (img.Width - img.Height) / 2;
            }
            else
            {
                s = img.Width;
                left = 0;
                top = (img.Height - img.Width) / 2;
            }

            Image desc = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(desc))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.DrawImage(img, new Rectangle(0, 0, size, size), new Rectangle(left, top, s, s), GraphicsUnit.Pixel);
            }
            return desc;
        }

        public static Image Crop(this Image img, int x, int y, int width, int height, int targetWidth, int targetHeight)
        {
            Image desc = new Bitmap(targetWidth, targetHeight);
            using (var g = Graphics.FromImage(desc))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.DrawImage(img, new Rectangle(0, 0, desc.Width, desc.Height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }
            return desc;
        }

        /// <summary>
        /// 高质量缩小图像尺寸,保证缩小后不模糊
        /// </summary>
        public static Image FixWidth(this Image img, int width)
        {
            if (width >= img.Width)
                return new Bitmap(img);

            int height = (int)(1.0 * width * img.Height / img.Width);
            Image desc = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(desc))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.DrawImage(img, new Rectangle(0, 0, desc.Width, desc.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            }
            return desc;
        }

        public static Stream SaveAsJpeg(this Image img, int quality = 90)
        {
            MemoryStream ms = new MemoryStream();
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);

            myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            img.Save(ms, myImageCodecInfo, myEncoderParameters);

            return ms;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}