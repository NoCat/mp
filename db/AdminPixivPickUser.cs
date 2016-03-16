using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class AdminPixivUser
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PackageID { get; set; }
        public virtual Package Package { get; set; }
        public DateTime LastPickTime { get; set; }
        public string UserName { get; set; }
        public AdminPixivUserStates State { get; set; }
    }

    public enum AdminPixivUserStates:byte
    {
        Wait=0,
        Pick=1,
        Skip=2
    }
}
