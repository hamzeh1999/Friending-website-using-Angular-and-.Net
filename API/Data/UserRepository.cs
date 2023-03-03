using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.healper;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly IMapper _map;

        private readonly DataContext _context;
        public UserRepository(DataContext ctx, IMapper map)
        {
            _map = map;
            _context = ctx;
        }


        public async Task<AppUser> getUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }



        public async Task<PageList<MemberDTO>> getMemberAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            var minDob = DateTime.Today.AddYears(-userParams.maxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.minAge);

            query = query.Where(u => u.UserName != userParams.currentUserName);
            query = query.Where(u => u.gender == userParams.gender);
            query = query.Where(u => u.dateOfBirth >= minDob && u.dateOfBirth <= maxDob);

            query = userParams.orderBy switch
            {
                "created" => query.OrderByDescending(u => u.created),
                _ => query.OrderByDescending(u => u.lastActivity)
            };


            return await PageList<MemberDTO>.createAsync(query.ProjectTo<MemberDTO>(_map.ConfigurationProvider).AsNoTracking(),
             userParams.pageNumber, userParams.pageSize);

        }





        public async Task<MemberDTO> getMemberAsync(string userName)
        {
            // var ham=_context.AppUser.Where(x=>x.userName==userName);
            return await _context.Users.Where(x => x.UserName == userName)
         .ProjectTo<MemberDTO>(_map.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<AppUser> getUserByUserNameAsync(string username)
        {

         Console.WriteLine("😢😢😂   getUserByUserNameAsync");
            var user = await _context.Users.Include(t => t.photos).SingleOrDefaultAsync(x => x.UserName == username);
            // for(int i=0;i<user.photos.ToArray().Length;i++)
            // {
            //     Console.WriteLine("here"+user.photos.ToArray()[i].url);
            // }
            // Console.WriteLine("In Repository......"+user.photos.ToArray());
            return user;
        }




        public async Task<IEnumerable<AppUser>> getUsersAsync()
        {
            return await _context.Users.Include(p => p.photos).ToListAsync();
        }

       

        public void update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<string> getUserGender(string username)
        {
         return await _context.Users.Where(x=>x.UserName==username)
         .Select(x=>x.gender).FirstOrDefaultAsync(); 
        }
    }
}