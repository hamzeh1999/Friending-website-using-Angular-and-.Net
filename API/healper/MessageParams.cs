using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.healper
{
    public class MessageParams:PaginationParams
    {
        public string userName { get; set; }
        public string container { get; set; }="unread";
    }
}