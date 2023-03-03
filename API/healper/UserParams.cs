using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.healper
{
    public class UserParams:PaginationParams
    {
        public string currentUserName { set; get; }

        public string gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxAge { set; get; } = 150;

        public string orderBy{get; set;}="lastActive";

       

    }
}