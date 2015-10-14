using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Pick
    {
        public int ID { get; set; }
        public int DownloadID { get; set; }
        virtual public Download Download { get; set; }
        public int PackageID { get; set; }
        virtual public Package Package { get; set; }
        public int FromUrlID { get; set; }
        public string Description { get; set; }
    }
}
