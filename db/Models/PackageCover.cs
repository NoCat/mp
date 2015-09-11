using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public class PackageCover
    {
        public int ID { get; set; }
        public int PackageID { get; set; }
        virtual public Package Package { get; set; }
        public int CoverID { get; set; }
        virtual public Image Cover { get; set; }
    }
}
