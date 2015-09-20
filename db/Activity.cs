using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Activity
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        virtual public User Sender { get; set; }
        public int RecieverID { get; set; }
        virtual public User Reciever { get; set; }
        public int Target { get; set; }
        public int Addition { get; set; }
        public ActivityTypes Type { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
