using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace db.Models
{
    public class User
    {
        public int ID { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Head { get; set; }    
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime LastGetActivityTime { get; set; }
        public DateTime LastGetMessageTime { get; set; }        

        public User()
        {
            Head = "default";
        }
    }
}
