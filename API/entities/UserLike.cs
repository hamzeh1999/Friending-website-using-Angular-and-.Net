using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.entities
{
    public class UserLike
    {
        public AppUser sourceUser{set; get;}

        public int sourceUserId{set; get;}


        public AppUser likedUser{set; get;}
         
        public int likedUserId{set; get;}

        
    }        

        }