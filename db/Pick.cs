using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Pick
    {
        public int ID { get; set; }
        public int PackageID { get; set; }
        virtual public Package Package { get; set; }
        public int SourceUrlID { get; set; }
        virtual public Url SourceUrl { get; set; }
        public int FromUrlID { get; set; }
        virtual public Url FromUrl { get; set; }
        public string Description { get; set; }
    }
}
