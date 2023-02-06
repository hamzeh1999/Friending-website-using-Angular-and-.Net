
using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.entities;
using Microsoft.AspNetCore.Authorization;
using API.interfaces;
using API.DTO;
using AutoMapper;
using System;
using System.Security.Claims;
using API.Extensions;
using API.healper;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        // private readonly DataContext _context;
        private readonly IUserRepository _user;
        private readonly IMapper _mapper;
        private readonly IPhotoServices _photo;

        public UsersController(IUserRepository user, IMapper mapper, IPhotoServices photo)
        {
            _photo = photo;
            _mapper = mapper;
            _user = user;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParams usersParams)
        {
            var user=await _user.getUserByUserNameAsync(User.getUserName());

            usersParams.currentUserName=user.userName;
            if(string.IsNullOrEmpty(usersParams.gender))
            usersParams.gender=user.gender=="male"?"female":"male";
            var users = await _user.getMemberAsync(usersParams);
            
            
            
            Response.addPaginationHeader(users.currentPage, users.pageSize, users.totalCount, users.totalPages);




            return Ok(users);


        }

       
        [HttpGet("{userName}", Name = "getUser")]
        public async Task<ActionResult<MemberDTO>> GetUsersByName(string userName)
        {


            return await _user.getMemberAsync(userName);


        }

        [HttpPut]
        public async Task<ActionResult> updateUser(updateMemberDTO member)
        {
            var username = User.getUserName();
            var user = await _user.getUserByUserNameAsync(username);
            _mapper.Map(member, user);
            _user.update(user);
            if (await _user.saveAllAsync()) return NoContent();
            return BadRequest("Failed to update User !");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _user.getUserByUserNameAsync(User.getUserName());
            var result = await _photo.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo1 = new Photo
            {
                url = result.SecureUrl.AbsoluteUri,
                publicId = result.PublicId
            };

            if (user.photos.Count == 0)
            {
                photo1.isMan = true;
            }
            user.photos.Add(photo1);

            if (await _user.saveAllAsync())
            {

                return CreatedAtRoute("getUser", new { userName = user.userName }, _mapper.Map<PhotoDTO>(photo1));
            }
            //return _mapper.Map<PhotoDTO>(photo1);

            return BadRequest("Problem Adding Photo");
        }


        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> setMainPhoto(int photoId)
        {
            var user = await _user.getUserByUserNameAsync(User.getUserName());
            var photo = user.photos.FirstOrDefault(x => x.id == photoId);
            if (photo.isMan) return BadRequest("This is already main photo");
            var currentMain = user.photos.FirstOrDefault(x => x.isMan);
            if (currentMain != null) currentMain.isMan = false;
            photo.isMan = true;
            if (await _user.saveAllAsync()) return NoContent();
            return BadRequest("Failed to set main Photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> deletePhoto(int photoId)
        {
            Console.WriteLine("haaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            var user = await _user.getUserByUserNameAsync(User.getUserName());
            var photo = user.photos.FirstOrDefault(x => x.id == photoId);
            if (photo == null) return NotFound();
            if (photo.isMan) return BadRequest("You Can't Delete your Main photo");
            if (photo.publicId != null)
            {
                var result = await _photo.DeletePhotoAsync(photo.publicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.photos.Remove(photo);
            if (await _user.saveAllAsync()) return Ok();
            return BadRequest("Failed to Delete Photo");
        }
    }
}