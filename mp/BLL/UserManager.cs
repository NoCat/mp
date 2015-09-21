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
    }
}