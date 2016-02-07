using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;
using mp.BLL;

namespace mp.Models
{
    public class PackageInfo
    {
        ManagerCollection  DB
        {
            get
            {
                return Security.Manager;
            }
        }

        Package _package = null;
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri HomePage { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }

        int _imageCount = -1;
        public int ImageCount
        {
            get
            {
                if (_imageCount == -1)
                {
                    _imageCount = DB.Images.Items.Where(i => i.PackageID == _package.ID).Count();
                }
                return _imageCount;
            }
        }

        ImageInfo _cover = null;
        public ImageInfo Cover
        {
            get
            {
                if (_cover == null)
                {
                    if (_package.CoverID != 0)
                    {
                        var image = DB.Images.Find(_package.CoverID);
                        _cover = new ImageInfo(image);
                    }
                }
                return _cover;
            }
        }

        int _followerCount = -1;
        public int FollowerCount
        {
            get
            {
                if (_followerCount == -1)
                {
                    _followerCount = DB.Followings.Items.Where(f => f.Type == FollowingTypes.Package && f.Info == _package.ID).Count();
                }
                return _followerCount;
            }
        }

        UserInfo _user = null;
        public UserInfo User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserInfo(_package.User);
                }
                return _user;
            }
        }

        public PackageInfo(Package package)
        {
            _package = package;
            Title = package.Title;
            Description = package.Description;
            HomePage = new Uri("/package/" + package.ID, UriKind.Relative);
            ID = package.ID;
            UserID = package.UserID;
        }
    }
}