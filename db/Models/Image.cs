using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace db.Models
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
        public string Url { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public int Via { get; set; }
        [MaxLength(100)]
        public string Host { get; set; }
    }
}
