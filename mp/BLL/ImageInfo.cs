using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class ImageInfo
    {
        Image _image = null;
        public ImageInfo(Image image)
        {
            _image = image;
            Description = _image.Description;
        }
        UserInfo _user = null;
        public UserInfo User
        {
            get
            {
                if (_user==null)
                {
                    _user = new UserInfo(_image.User);
                }
                return _user;
            }
        }


        PackageInfo _package = null;
        public PackageInfo Package
        {
            get
            {
                if (_package==null)
                {
                    _package = new PackageInfo(_image.Package);
                }
                return _package;
            }
        }

        public string Description { get; set; }


        Thumb _thumbFW236 = null;
        public Thumb ThumbFW236
        {
            get
            {
                if (_thumbFW236 == null)
                    _thumbFW236 = new Thumb(_image, "fw", 236);
                return _thumbFW236;
            }
        }

        Thumb _thumbSQ236 = null;
        public Thumb ThumbSQ236
        {
            get
            {
                if (_thumbSQ236 == null)
                    _thumbSQ236 = new Thumb(_image, "sq", 236);
                return _thumbSQ236;
            }
        }

        Thumb _thumbSQ75 = null;
        public Thumb ThumbSQ75
        {
            get
            {
                if (_thumbSQ75 == null)
                    _thumbSQ75 = new Thumb(_image, "sq", 75);
                return _thumbSQ75;
            }
        }

        Thumb _thumbFW658 = null;
        public Thumb ThumbFW658
        {
            get
            {
                if (_thumbFW658 == null)
                    _thumbFW658 = new Thumb(_image, "fw", 658);
                return _thumbFW658;
            }
        }

        Thumb _thumbFW78 = null;
        public Thumb ThumbFW78
        {
            get
            {
                if (_thumbFW78 == null)
                    _thumbFW78 = new Thumb(_image, "fw", 78);
                return _thumbFW78;
            }
        }
    }

    public class Thumb
    {
        public Thumb(Image img, string type, int size)
        {
            int width = size;
            int height = size;
            if (type == "fw")
                height = (int)(1.0 * width / img.File.Width * img.File.Height);
            Url = new Uri(Configs.ImageHost, string.Format("{0}_{1}{2}.jpg", img.File.MD5, type, size));
            Width = width;
            Height = height;
        }
        public Uri Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

}