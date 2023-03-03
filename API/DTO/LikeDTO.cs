using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class LikeDTO
    {
        public int id { get; set; }

        public string userName { get; set; }

        public int age { get; set; }

        public string knownAs{get; set;}

        public string city  { get; set; }

        public string photoUrl { get; set; }
    }
}