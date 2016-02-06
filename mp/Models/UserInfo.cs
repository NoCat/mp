using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;
using mp.BLL;

namespace mp.Models
{
    public class UserInfo
    {
        ManagerCollection Manager
        {
            get
            {
                return Security.Manager;
            }
        }
        User _user = null;
        public Uri Head { get; set; }
        public Uri BigHead { get; set; }
        public Uri HomePage { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }

        int _packageCount = -1;
        public int PackageCount
        {
            get
            {
                if (_packageCount == -1)
                    _packageCount = Manager.Packages.Items.Where(p => p.UserID == _user.ID).Count();
                return _packageCount;
            }
        }

        int _praiseCount = -1;
        public int PraiseCount
        {
            get
            {
                if (_praiseCount == -1)
                    _praiseCount = Manager.Praises.Items.Where(p => p.UserID == _user.ID).Count();
                return _praiseCount;
            }
        }

        int _followingCount = -1;
        public int FollowingCount
        {
            get
            {
                if (_followingCount == -1)
                    _followingCount = Manager.Followings.Items.Where(f => f.UserID == _user.ID).Count();
                return _followingCount;
            }
        }

        int _imageCount = -1;
        public int ImageCount
        {
            get
            {
                if (_imageCount==-1)
                {
                    _imageCount = Manager.Images.Items.Where(i => i.UserID == _user.ID).Count();
                }
                return _imageCount;
            }
        }

        int _followerCount = -1;
        public int FollowerCount
        {
            get
            {
                if(_followerCount==-1)
                {
                    _followerCount = Manager.Followings.Items.Where(f => f.Type == FollowingTypes.User && f.Info == _user.ID).Count();
                }
                return _followerCount;
            }
        }

        public UserInfo(User user)
        {
            _user = user;
            var headId = 0;
            if (user.UseDefaultHead == false)
                headId = user.ID;
            Head = new Uri(Configs.ImageHost, string.Format("avt/{0}", headId));
            BigHead = new Uri(Configs.ImageHost, string.Format("avt/{0}_big", headId));
            HomePage = new Uri("/user/" + user.ID, UriKind.Relative);
            Name = user.Name;
            ID = user.ID;
        }
    }

}