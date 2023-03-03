using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class CreateMessageDTO
    {
        public string recipientUserName  { get; set; }
        public string content { get; set; }
    }
}