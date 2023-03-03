using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.healper
{
    public class LikesParams:PaginationParams
    {
        public int userId { get; set; }
        public string predict { get; set; }

    }
}