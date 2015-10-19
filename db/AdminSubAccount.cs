using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class AdminSubAccount
    {
        public int ID { get; set; }
        public int AdminUserID { get; set; }
        virtual public AdminUser AdminUser { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
    }
}
