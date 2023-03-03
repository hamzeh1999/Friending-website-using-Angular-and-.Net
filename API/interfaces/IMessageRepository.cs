using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.healper;

namespace API.interfaces
{
    public interface IMessageRepository
    {

        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> getConnection(string connectionId);
        Task<Group> getMessageGroup(string groupName);
        Task<Group> getGroupForConnection(string connectionId);
        void addMessage(Message message);
        void deleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PageList<MessageDTO>> getMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> getMessagesThread(string currentUserUserName,string recipientUserName);
       
    }
}