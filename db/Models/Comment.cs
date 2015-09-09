using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace db.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int ImageID { get; set; }
        virtual public Image Image { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
