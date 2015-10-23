using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;

namespace mp
{
    public  class ConfigManager
    {
        MiaopassContext _db=new MiaopassContext();

        public string this[string name]
        {
            get
            {
                var res = _db.Configs.Where(c => c.Name == name).FirstOrDefault();
                if (res == null)
                    return null;
                return res.Value;
            }
            set
            {
                var res=_db.Configs.Where(c => c.Name == name).FirstOrDefault();
                if (res == null)
                    _db.ConfigInsert(new Config { Name = name, Value = value });
                else
                {
                    res.Value=value;
                    _db.ConfigUpdate(res);
                }
            }
        }
    }
}
