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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountControler : BaseApiController
    {
        private readonly ITokenServices _token;
        private IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        
        // private readonly DataContext _context;

        public AccountControler(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
        ,ITokenServices token, IMapper map)
        {
            // _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = map;
            _token = token;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser( RegisterDto registerUser)
        {
            if (await UserExists(registerUser.userName)) return BadRequest("userName is taken");
            var user = _mapper.Map<AppUser>(registerUser);

            //using var hmac = new HMACSHA512();

            // Console.WriteLine("UserName:::" + user.UserName);
            user.UserName = registerUser.userName.ToLower();
            // user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password));
            // user.passwordSalt = hmac.Key;
            var result=await _userManager.CreateAsync(user,registerUser.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();
            var roleResult=await _userManager.AddToRoleAsync(user,"Member");
            if(!roleResult .Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                userName = user.UserName,
                token = await _token.CreateToken(user),
                photoUrl=user.photos.FirstOrDefault(x=>x.isMan)?.url,
                knownAs = user.knownAs,
                gender = user.gender

            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> loginUser( loginDTO userEnter)
        {
            var user = await _userManager.Users.Include(p => p.photos).SingleOrDefaultAsync(x => x.UserName == userEnter.userName.ToLower());
            if (user == null) return Unauthorized("Invalid UserName");
        //     using var hmac = new HMACSHA512(user.passwordSalt);
        //    var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userEnter.password));
        //     for (int i = 0; i < ComputeHash.Length; i++)
        //     {
        //         if (ComputeHash[i] != user.passwordHash[i]) return Unauthorized("Invalid Password");
        //     }

            var result=await _signInManager.CheckPasswordSignInAsync(user,userEnter.password,false);
            if(!result.Succeeded) 
            return Unauthorized();
            Console.WriteLine("Success : "+result.Succeeded);
            return new UserDto
            {
                userName = user.UserName,
                token = await _token.CreateToken(user),
                photoUrl = user.photos.FirstOrDefault(x => x.isMan)?.url,
                knownAs = user.knownAs,
                gender = user.gender

            };

        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == userName);
        }
    }
}