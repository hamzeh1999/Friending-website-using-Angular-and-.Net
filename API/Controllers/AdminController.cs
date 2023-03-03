using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{

    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManger;
        public AdminController(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("user-with-Roles")]
        public async Task<ActionResult>  getUserWithRoles()
        {
            var users= await _userManger.Users.
            Include(u=>u.UserRoles).ThenInclude(r=>r.Role)
            .OrderBy(u=>u.UserName)
            .Select(u=>new {
                u.Id,
                UserName=u.UserName,
                Roles=u.UserRoles.Select(r=>r.Role.Name).ToList()
            })
            .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{userName}")]
        public async Task<ActionResult> editRoles(string userName,[FromQuery] string roles)
        {
            var selectedRoes= roles.Split(",").ToArray();
            var user=await _userManger.FindByNameAsync(userName);

            if(user==null) return NotFound("Could Not Find User");

            var userRoles=await _userManger.GetRolesAsync(user);
            
            var result=await _userManger.AddToRolesAsync(user,selectedRoes.Except(userRoles));
            if(!result.Succeeded) BadRequest("Failed to add Roles");
            
            result=await _userManger.RemoveFromRolesAsync(user,userRoles.Except(selectedRoes));
            return Ok(_userManger.GetRolesAsync(user));
        
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photo-to-Roles")]
        public  ActionResult getPhotoForModeration()
        {
            return Ok("Only Admins Or Moderators can see this ");
        }

    }
}