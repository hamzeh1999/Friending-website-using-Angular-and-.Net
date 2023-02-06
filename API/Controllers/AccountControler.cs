using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountControler : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenServices _token;
        private IMapper _mapper;

        public AccountControler(DataContext context, ITokenServices token, IMapper map)
        {
            _mapper = map;
            _token = token;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] RegisterDto registerUser)
        {
            if (await UserExists(registerUser.userName)) return BadRequest("userName is taken");
            var user = _mapper.Map<AppUser>(registerUser);

            using var hmac = new HMACSHA512();

            Console.WriteLine("UserName:::" + user.userName);
            user.userName = registerUser.userName.ToLower();
            user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password));
            user.passwordSalt = hmac.Key;



            _context.AppUser.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                userName = user.userName,
                token = _token.CreateToken(user),
                knownAs = user.knownAs,
                gender = user.gender

            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> loginUser([FromBody] loginDTO userEnter)
        {
            var user = await _context.AppUser.Include(p => p.photos).SingleOrDefaultAsync(x => x.userName == userEnter.userName);
           Console.WriteLine("user::::::::::::::::::::::::::::"+user);
            if (user == null) return Unauthorized("Invalid UserName");
            using var hmac = new HMACSHA512(user.passwordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userEnter.password));
            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != user.passwordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                userName = user.userName,
                token = _token.CreateToken(user),
                photoUrl = user.photos.FirstOrDefault(x => x.isMan)?.url,
                knownAs = user.knownAs,
                gender = user.gender

            };

        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.AppUser.AnyAsync(x => x.userName == userName);
        }
    }
}