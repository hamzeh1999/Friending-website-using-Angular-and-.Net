using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.entities
{
    public class Message
    {
        public int id { get; set; }
        public int senderId { get; set; }
        public string senderUserName { get; set; }
        public AppUser sender { get; set; }




        public int recipientId { get; set; }
        public string recipientUserName { get; set; }
        public AppUser recipient { get; set; }




        public string content { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime messageSent { get; set; }=DateTime.UtcNow;





        public bool senderDeleted{get; set;}
        public bool recipientDeleted { get; set; }
    }
}