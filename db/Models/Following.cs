using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public class Following
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public FollowingTypes Type { get; set; }
        public int Info { get; set; }
    }
}
