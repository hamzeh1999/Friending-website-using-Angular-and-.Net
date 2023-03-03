using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.Extensions;
using API.healper;
using API.interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext data)
        {
            _context = data;

        }

        public async Task<UserLike> getUserlike(int sourceUserId, int likedUserId)
        {
            return await _context.likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PageList<LikeDTO>> getUserLikes(LikesParams likeParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.likes.AsQueryable();
            if (likeParams.predict == "liked")
            {
                likes = likes.Where(like => like.sourceUserId ==likeParams.userId);
                users = likes.Select(like => like.likedUser);
            }

            
            if (likeParams.predict == "likedBy")
            {
                likes = likes.Where(like => like.likedUserId == likeParams.userId);
                users = likes.Select(like => like.sourceUser);
            }
            var likedUsers= users.Select(user => new LikeDTO
            {
                id = user.Id,

                userName = user.UserName,

                age = 23,//user.dateOfBirth.calculateAge(),

                knownAs = user.knownAs,

                city = user.city,

                photoUrl = user.photos.FirstOrDefault(x => x.isMan).url,
            });
            return await PageList<LikeDTO>.createAsync(likedUsers,likeParams.pageNumber,likeParams.pageSize);
        }

        public async Task<AppUser> getUserWithLikes(int userId)
        {
            return await _context.Users
            .Include(x => x.likedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);

        }
    }
}