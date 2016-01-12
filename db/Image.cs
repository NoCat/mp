using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mp.DAL
{
    public class Image
    {
        public int ID { get; set; }
        public int PackageID { get; set; }
        virtual public Package Package { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public int FileID { get; set; }
        virtual public File File { get; set; }
        public DateTime CreatedTime { get; set; }
        public int FromUrlID { get; set; }
        public string Description { get; set; }
        public int PraiseCount { get; set; }

        public Image()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
