using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using mp.DAL;
using System.Drawing.Imaging;

namespace mp.BLL
{
    public class FileManager
    {
        MiaopassContext _db;
        public FileManager(MiaopassContext db)
        {
            _db = db;
        }

        public mp.DAL.File Create(Stream s, string md5)
        {
            var file = _db.Files.Where(i => i.MD5 == md5).FirstOrDefault();
            if (file == null)
            {
                s.Position = 0;
                try
                {
                    using (System.Drawing.Image bitmap = System.Drawing.Image.FromStream(s))
                    {
                        //检查图像是否为jpg,png,bmp
                        if (bitmap.RawFormat.Equals(ImageFormat.Bmp) == false && bitmap.RawFormat.Equals(ImageFormat.Png) == false && bitmap.RawFormat.Equals(ImageFormat.Jpeg) == false)
                            return null;

                        //上传原始的(如果格式非jpg,则转换成jpg,如果图片大于800w像素,则压缩小于800w像素)图片
                        int threshold = 8000000;
                        int pixels = bitmap.Width * bitmap.Height;
                        int width = bitmap.Width;
                        int height = bitmap.Height;

                        if (pixels > threshold)
                        {
                            int w = (int)(bitmap.Width / Math.Sqrt(1.0 * pixels / threshold));
                            using (var t = bitmap.FixWidth(w))
                            {
                                OssFile.Create(md5 + ".jpg", t.SaveAsJpeg());
                                width = t.Width;
                                height = t.Height;
                            }
                        }
                        else
                        {
                            OssFile.Create(md5 + ".jpg", bitmap.SaveAsJpeg());
                        }


                        //上传236定宽
                        using (var t = bitmap.FixWidth(236))
                        {
                            OssFile.Create(md5 + "_fw236.jpg", t.SaveAsJpeg());
                        }

                        //上传236方形
                        using (var t = bitmap.Square(236))
                        {
                            OssFile.Create(md5 + "_sq236.jpg", t.SaveAsJpeg());
                        }

                        //上传75方形
                        using (var t = bitmap.Square(75))
                        {
                            OssFile.Create(md5 + "_sq75.jpg", t.SaveAsJpeg());
                        }

                        //上传658定宽
                        using (var t = bitmap.FixWidth(658))
                        {
                            OssFile.Create(md5 + "_fw658.jpg", t.SaveAsJpeg());
                        }

                        //上传78定宽
                        using (var t = bitmap.FixWidth(78))
                        {
                            OssFile.Create(md5 + "_fw78.jpg", t.SaveAsJpeg());
                        }

                        //写入数据库
                        file = new mp.DAL.File() { MD5 = md5, Height = height, Width = width };
                        _db.Insert(file);
                    }
                }
                catch { }
            }
            return file;
        }

    }
}