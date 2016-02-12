using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public  class AdminPixivWork
    {
        public int ID { get; set; }
        public int PixivWorkID { get; set; }
        public string Description { get; set; }
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
    }
}
