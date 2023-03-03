using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.entities;
using API.DTO;
using API.healper;

namespace API.interfaces
{
    public interface IUserRepository
    {
        void update(AppUser user);
        Task<IEnumerable<AppUser>> getUsersAsync();

        public  Task<PageList<MemberDTO>> getMemberAsync(UserParams userParams);

        public  Task<MemberDTO> getMemberAsync(string userName);
        public  Task<AppUser> getUserByIdAsync(int id );
        Task<AppUser> getUserByUserNameAsync(string userName);
        Task<string>getUserGender(string username);
    }
}