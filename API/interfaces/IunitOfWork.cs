using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.interfaces
{
    public interface IunitOfWork
    {
        IUserRepository UserRepository{get;}

        IMessageRepository MessageRepository{get;}
        ILikeRepository LikeRepository{get;}

        Task<bool> Complete();
        bool HasChanges();
    }
}