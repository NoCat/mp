using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace db.Models
{
    public class Tag
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Text { get; set; }
    }
}
