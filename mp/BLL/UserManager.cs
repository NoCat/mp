using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class UserManager
    {
        MiaopassContext _db;
        public UserManager(MiaopassContext db)
        {
            _db = db;
        }

        public static UserInfo GetUserInfo(User user)
        {
            return new UserInfo(user);
        }

        public class UserInfo
        {
            User _user = null;

            UserHead _head = null;
            public UserHead Head
            {
                get
                {
                    if (_head == null)
                        _head = new UserHead(_user);
                    return _head;
                }
            }

            public UserInfo(User user)
            {
                _user = user;
            }
        }

        public class UserHead
        {
            public Uri NormalSize { get; set; }
            public Uri BigSize { get; set; }
            public UserHead(User user)
            {
                NormalSize = new Uri(Configs.ImageHost, string.Format("avt/{0}.jpg", user.ID));
                BigSize = new Uri(Configs.ImageHost, string.Format("avt/{0}_big.jpg", user.ID));
            }
        }
    }
}