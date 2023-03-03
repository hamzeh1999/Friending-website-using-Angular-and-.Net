using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.Extensions;
using API.healper;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _map;
        public MessageRepository(DataContext context, IMapper map)
        {
            _map = map;
            _context = context;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void addMessage(Message message)
        {
            _context.Message.Add(message);

        }
        public async Task<Connection> getConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
            //throw new NotImplementedException();
        }

        public async Task<Group> getMessageGroup(string groupName)
        {
            return await _context.Groups
            .Include(x=>x.Connections)
            .FirstOrDefaultAsync(x=>x.Name==groupName);
            //throw new NotImplementedException();
        }

        public void deleteMessage(Message message)
        {
            _context.Message.Remove(message);
        }



        public async Task<Message> GetMessage(int id)
        {
            return await _context.Message
            .Include(m => m.sender)
            .Include(m => m.recipient)
            .SingleOrDefaultAsync(x => x.id == id);
        }

        public async Task<PageList<MessageDTO>> getMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Message.OrderByDescending(m => m.messageSent)
            .ProjectTo<MessageDTO>(_map.ConfigurationProvider)
            .AsQueryable();

            query = messageParams.container switch
            {
                "Inbox" => query.Where(u => u.recipientUserName == messageParams.userName &&
                 u.recipientDeleted == false),
                "Outbox" => query.Where(u => u.senderUserName == messageParams.userName &&
                u.senderDeleted == false),
                _ => query.Where(u => u.recipientUserName == messageParams.userName &&
                    u.recipientDeleted == false && u.dateRead == null)
            };
        //    var message = query.ProjectTo<MessageDTO>(_map.ConfigurationProvider);
            return await PageList<MessageDTO>.createAsync(query, messageParams.pageNumber, messageParams.pageSize);

        }

        public async Task<IEnumerable<MessageDTO>> getMessagesThread(string currentUserUserName, string recipientUserName)
        {
            var messages = await _context.Message
            // .Include(u => u.sender).ThenInclude(p => p.photos)
            // .Include(u => u.recipient).ThenInclude(p => p.photos)
            .Where(m => m.recipient.UserName == currentUserUserName && m.recipientDeleted == false &&
            m.sender.UserName == recipientUserName ||
            m.recipient.UserName == recipientUserName && m.sender.UserName == currentUserUserName && m.senderDeleted == false
            ).OrderBy(m => m.messageSent)
            .MarkUnreadAsRead(currentUserUserName)
            .ProjectTo<MessageDTO>(_map.ConfigurationProvider)
            .ToListAsync();

            // var unReadMessage = messages
            // .Where(m => m.dateRead == null &&
            //  m.recipientUserName == currentUserUserName);
            // if (unReadMessage.Any())
            // {
            //     foreach (var message in unReadMessage)
            //     {
            //         message.dateRead = DateTime.UtcNow;
            //     }

                // await _context.SaveChangesAsync();
             //     }
            //return _map.Map<IEnumerable<MessageDTO>>(message)
               return messages;
        }



// public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername,
//             string recipientUsername)
//         {
//             var messages = await _context.Messages
//                 .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
//                         && m.Sender.UserName == recipientUsername
//                         || m.Recipient.UserName == recipientUsername
//                         && m.Sender.UserName == currentUsername && m.SenderDeleted == false
//                 )
//                 .MarkUnreadAsRead(currentUsername)
//                 .OrderBy(m => m.MessageSent)
//                 .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
//                 .ToListAsync();

//             return messages;
//         }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

     

        public async Task<Group> getGroupForConnection(string connectionId)
        {
           return await _context.Groups.Include(c=>c.Connections)
           .Where(c=>c.Connections.Any(x=>x.ConnectionId==connectionId))
           .FirstOrDefaultAsync();
        }
    }
}