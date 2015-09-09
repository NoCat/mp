using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace db.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        virtual public User Sender { get; set; }
        public int RecieverID { get; set; }
        virtual public User Reciever { get; set; }
        public int Target { get; set; }
        public int Addition { get; set; }
        public MessageTypes Type { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
