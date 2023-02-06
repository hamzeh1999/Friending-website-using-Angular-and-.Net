using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static  class DateTimeExtensions1
    {
        
        public static int calculateAge(this DateTime dob)
        {

            var today=DateTime.Today;
            var age=today.Year-dob.Year;
            if(dob.Date>today.AddYears(-age)) age--;
            return age; 
        }
    }
}
