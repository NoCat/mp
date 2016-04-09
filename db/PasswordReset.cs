using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class PasswordReset
    {
        public int ID { get; set; }
        public int UserID  { get; set; }
        virtual public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
