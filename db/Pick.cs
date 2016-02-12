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
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public int FromUrlID { get; set; }
        virtual public Url FromUrl { get; set; }
        public string Description { get; set; }
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
    }
}
