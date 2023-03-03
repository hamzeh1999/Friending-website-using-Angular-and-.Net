using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.entities
{
    public class AppUser:IdentityUser<int>
    {
       
        
        // public int Id { get; set; }
        // public string   UserName { get; set; } 

        


        // public byte[] PasswordHash { get; set; }

        // public byte[] passwordSalt { get; set; }


        public DateTime created { get; set; } = DateTime.Now;

        public DateTime lastActivity { get; set; } = DateTime.Now;

        public DateTime dateOfBirth { get; set; }

        public string knownAs { get; set; }

        public string gender { get; set; }

        public string introduction { get; set; }

        public string lookingFor { get; set; }

        public string interests { get; set; }


        public string country { get; set; }


        public ICollection<Photo> photos { get; set; }


        public string city { get; set; }

        public ICollection<UserLike> likedByUsers { get; set; }
        public ICollection<UserLike> likedUsers { set; get; }



        public ICollection<Message> messageSent { get; set; }
        public ICollection<Message> messageReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }

        // public ICollection<AppUserRole> UserRoles{get; set;}
    }
}