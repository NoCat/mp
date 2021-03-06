﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class ImageTag
    {
        public int ID { get; set; }
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
        public int TagID { get; set; }
        virtual public Tag Tag { get; set; }
    }
}
