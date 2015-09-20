using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace mp.DAL
{
    public class LoginCookie
    {
        [Key,MaxLength(40)]
        public string Value { get; set; }
        public DateTime Expire { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
    }
}
