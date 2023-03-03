using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.Extensions;
using API.healper;
using API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize]

    public class LikesController : BaseApiController
    {
       
        private readonly IunitOfWork _unintOfWork;

        public LikesController(IunitOfWork unintOfWork)
        {
            _unintOfWork = unintOfWork;
          
        }

        [HttpPost("{userName}")]
        public async Task<ActionResult> addLike(string userName)
        {
            var sourceUserId = User.getUserId();
            var likedUser = await _unintOfWork.UserRepository.getUserByUserNameAsync(userName);
            var sourceUser = await _unintOfWork.LikeRepository.getUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == userName) return BadRequest("You Can't Like Yourself");


            var userLike = await _unintOfWork.LikeRepository.getUserlike(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You are already like this User");
            userLike = new UserLike
            {
                sourceUserId = sourceUserId,
                likedUserId = likedUser.Id
            };
            sourceUser.likedUsers.Add(userLike);
            if (await _unintOfWork.Complete()) return Ok();

            return BadRequest("Failed To Return This User");

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> getUserLikes([FromQuery]LikesParams likesParams)
        {
            // Console.WriteLine("Hamzeh");
            likesParams.userId = User.getUserId();
            // Console.WriteLine("likes1 : " + likesParams.predict + "  " + likesParams.pageSize + " " + likesParams.pageNumber + "  " + likesParams.userId);

            var user = await _unintOfWork.LikeRepository.getUserLikes(likesParams);
            // Console.WriteLine("likes : " + likesParams.predict + "  " + likesParams.pageSize + " " + likesParams.pageNumber + "  " + likesParams.userId);
            Response.addPaginationHeader(user.currentPage,
             user.pageSize, user.totalCount, user.totalPages);

            return Ok(user);
        }

    }
}