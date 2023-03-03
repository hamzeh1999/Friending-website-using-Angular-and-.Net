using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class  QueryableExtensions
    {
        
      public static IQueryable<Message> MarkUnreadAsRead(this IQueryable<Message> query, string currentUsername)
        {
            Console.WriteLine("username : "+currentUsername);
            var unreadMessages = query.Where(m => m.dateRead == null
                && m.recipientUserName == currentUsername);

                Console.WriteLine("UnReadMessage --------->: "+unreadMessages.ToQueryString());

            if (unreadMessages.Any())
            {                Console.WriteLine("unreadMessages.Any() "+unreadMessages);

                foreach (var message in unreadMessages)
                {
                    Console.WriteLine("message "+ message.dateRead);

                    message.dateRead = DateTime.UtcNow;
                }
            }

            return query;
        }
}
}