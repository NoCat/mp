using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public  class AdminPixivWorkTag
    {
        public int ID { get; set; }
        public int WorkID { get; set; }
        virtual public AdminPixivWork Work { get; set; }
        public int TagID { get; set; }
    }
}
