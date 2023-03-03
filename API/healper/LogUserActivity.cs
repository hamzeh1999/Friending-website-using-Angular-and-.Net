using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.healper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var  resultContext=await next();
           //int  userId=7;
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return ;
            var userId=resultContext.HttpContext.User.getUserId();
            Console.WriteLine("Hamzeh Ghabashneh to confirm it is me 😂");
            var repo=resultContext.HttpContext.RequestServices.GetService<IunitOfWork>();
            var user=await repo.UserRepository.getUserByIdAsync(userId);
            user.lastActivity=DateTime.UtcNow;
            await repo.Complete();
        }
    }
}