using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace mp.DAL
{
    public class Package
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public bool HasCover { get; set; }
        public int CoverID { get; set; }
    }
}
