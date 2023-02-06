using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string userName { get; set; }

        // public int age { get; set; }

        

        public byte[] passwordHash { get; set; }

        public byte[] passwordSalt { get; set; }

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



        // public int getAge()
        // {
        //     // var today = DateTime.Today;
        //     // var age = DateTime.Today.Year - dateOfBirth.Year;
        //     // if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
        //     // return age;
        //      return dateOfBirth.calculateAge();
        //    // return 20;
        // }

    }
}