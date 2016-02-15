using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Resave
    {
        public int ID { get; set; }
        public int Parent { get; set; }
        public int Child { get; set; }
        public int PathLength { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
