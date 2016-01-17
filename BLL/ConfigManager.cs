using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;

namespace mp.BLL
{
    public class ConfigManager:ManagerBase<Config>
    {
        public ConfigManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public string this[string name]
        {
            get
            {
                return  DB.Configs.Where(c => c.Name == name).Select(c=>c.Value).FirstOrDefault();                
            }
            set
            {
                var res = DB.Configs.Where(c => c.Name == name).FirstOrDefault();
                if (res == null)
                    DB.Add(new Config { Name = name, Value = value });
                else
                {
                    res.Value=value;
                    DB.Update(res);
                }
            }
        }
    }
}
