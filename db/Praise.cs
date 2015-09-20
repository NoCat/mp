using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Praise
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
    }
}
