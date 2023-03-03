
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
        private readonly IMapper _mapper;
        private readonly IPhotoServices _photo;
        private readonly IunitOfWork _unitOfWork;

        public UsersController(IunitOfWork unitOfWork , IMapper mapper, IPhotoServices photo)
        {
            _unitOfWork = unitOfWork;
            _photo = photo;
            _mapper = mapper;
          
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParams usersParams)
        {
            // var user = await _unitOfWork.UserRepository.getUserByUserNameAsync(User.getUserName());
            var gender = await _unitOfWork.UserRepository.getUserGender(User.getUserName());
            usersParams.currentUserName = User.getUserName();
            
            if (string.IsNullOrEmpty(usersParams.gender))
                usersParams.gender = gender == "male" ? "female" : "male";
            var users = await _unitOfWork.UserRepository.getMemberAsync(usersParams);



            Response.addPaginationHeader(users.currentPage, users.pageSize, users.totalCount, users.totalPages);




            return Ok(users);


        }

        [Authorize(Roles = "Member")]

        [HttpGet("{userName}", Name = "getUser")]
        public async Task<ActionResult<MemberDTO>> GetUsersByName(string userName)
        {


            return await _unitOfWork.UserRepository.getMemberAsync(userName);


        }

        [HttpPut]
        public async Task<ActionResult> updateUser(updateMemberDTO member)
        {
            var username = User.getUserName();
            var user = await _unitOfWork.UserRepository.getUserByUserNameAsync(username);

            _mapper.Map(member, user);
           
            _unitOfWork.UserRepository.update(user);
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update User !");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.getUserByUserNameAsync(User.getUserName());
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

            if (await _unitOfWork.Complete())
            {

                return CreatedAtRoute("getUser", new { userName = user.UserName }, _mapper.Map<PhotoDTO>(photo1));
            }
            //return _mapper.Map<PhotoDTO>(photo1);

            return BadRequest("Problem Adding Photo");
        }


        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> setMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.getUserByUserNameAsync(User.getUserName());
            var photo = user.photos.FirstOrDefault(x => x.id == photoId);
            if (photo.isMan) return BadRequest("This is already main photo");
            var currentMain = user.photos.FirstOrDefault(x => x.isMan);
            if (currentMain != null) currentMain.isMan = false;
            photo.isMan = true;
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to set main Photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> deletePhoto(int photoId)
        {
            Console.WriteLine("haaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            var user = await _unitOfWork.UserRepository.getUserByUserNameAsync(User.getUserName());
            var photo = user.photos.FirstOrDefault(x => x.id == photoId);
            if (photo == null) return NotFound();
            if (photo.isMan) return BadRequest("You Can't Delete your Main photo");
            if (photo.publicId != null)
            {
                var result = await _photo.DeletePhotoAsync(photo.publicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.photos.Remove(photo);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to Delete Photo");
        }
    }
}