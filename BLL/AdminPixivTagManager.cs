using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class AdminPixivTagManager:ManagerBase<AdminPixivTag>
    {
        public AdminPixivTagManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }
    }
}
