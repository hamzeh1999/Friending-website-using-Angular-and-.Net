using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.entities;

namespace API.interfaces
{
    public interface ITokenServices
    {
        string CreateToken(AppUser user);
    }
}