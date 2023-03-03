using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.DTO
{
    public class MessageDTO
    {
        public int id { get; set; }
        public int senderId { get; set; }
        public string senderUserName { get; set; }
        public string senderPhotoUrl { get; set; }




        public int recipientId { get; set; }
        public string recipientUserName { get; set; }
        public string recipientPhotoUrl { get; set; }




        public string content { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime messageSent { get; set; } 


        [JsonIgnore]
        public bool recipientDeleted { get; set; }
        [JsonIgnore]   //System.Text.Json--------------------->It means will not send to Client(Angular)

        public bool senderDeleted { get; set; }



    }
}