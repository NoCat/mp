using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class AdminPixivPickUser
    {
        public int ID { get; set; }
        public int PixivUserID { get; set; }
        public int PackageID { get; set; }
        public virtual Package Package { get; set; }
        public DateTime LastPickDate { get; set; }
    }
}
