using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;

namespace mp.DAL
{
    public class StoredProcedure
    {
        MiaopassContext _db = null;

        public StoredProcedure (MiaopassContext db)
        {
            _db = db;
        }
    }
}
