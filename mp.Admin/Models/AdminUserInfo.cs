using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.Admin.Models
{
    public class AdminUserInfo
    {
        private AdminUser _adminUser;
        public int ID { get; set; }
        public string Name { get; set; }

        public AdminUserInfo(AdminUser adminUser)
        {
            _adminUser = adminUser;
            ID = adminUser.ID;
            Name = adminUser.Name;
        }
    }
}