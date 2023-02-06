using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class UserDto
    {
        public string userName { get; set; }
        public string token { get; set; }
        public string knownAs { get; set; }
        public string gender { get; set; }

        public string photoUrl { get; set; }
    }
}