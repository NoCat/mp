using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;
using System.Data.Entity;

namespace mp.BLL
{
    public class ManagerBase
    {
        MiaopassContext _db;
        protected MiaopassContext DB
        {
            get
            {
                if (_db == null)
                    _db = new MiaopassContext();
                return _db;
            }
        }

        public ManagerBase() { }
        public ManagerBase(MiaopassContext db) { _db = db; }
    }
}
