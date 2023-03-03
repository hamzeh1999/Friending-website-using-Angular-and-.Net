using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.healper;

namespace API.interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> getUserlike(int sourceUserId,int likedUserId);
        Task<AppUser> getUserWithLikes(int userId);
        Task<PageList<LikeDTO>>getUserLikes(LikesParams likesParams);
    }
}