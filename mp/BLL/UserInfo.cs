using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class UserInfo
    {
        MiaopassContext _db = new MiaopassContext();
        User _user = null;
        public Uri Head { get; set; }
        public Uri BigHead { get; set; }
        public Uri HomePage { get; set; }
        public string Name { get; set; }

        int _imageCount = -1;
        public int ImageCount
        {
            get
            {
                if (_imageCount==-1)
                {
                    _imageCount = _db.Images.Where(i => i.UserID == _user.ID).Count();
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
                    _followerCount = _db.Followings.Where(f => f.Type == FollowingTypes.User && f.Info == _user.ID).Count();
                }
                return _followerCount;
            }
        }

        public UserInfo(User user)
        {
            _user = user;
            Head = new Uri(Configs.ImageHost, string.Format("avt/{0}.jpg", user.ID));
            BigHead = new Uri(Configs.ImageHost, string.Format("avt/{0}_big.jpg", user.ID));
            HomePage = new Uri("/user/" + user.ID, UriKind.Relative);
            Name = user.Name;
        }
    }

}