using System;
using System.Collections.Generic;
using System.Linq;
using API.entities;

namespace API.interfaces
{
    public interface ITokenServices
    {
        Task<string> CreateToken(AppUser user);
    }
}