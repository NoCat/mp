using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.Admin.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public UserInfo(User user)
        {
            ID = user.ID;
            Email = user.Email;
            Name = user.Name;
        }
    }
}