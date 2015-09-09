using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public class ImageTag
    {
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
        public int TagID { get; set; }
        virtual public Tag Tag { get; set; }
    }
}
