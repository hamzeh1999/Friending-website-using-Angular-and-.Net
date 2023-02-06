using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.healper
{
    public class UserParams
    {
        public string currentUserName { set; get; }

        public string gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxAge { set; get; } = 150;

        public string orderBy{get; set;}="lastActive";

        private const int MaxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}