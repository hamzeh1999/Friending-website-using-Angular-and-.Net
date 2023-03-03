using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using API.DTO;
using API.entities;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly IunitOfWork _unitOfWork;
        private readonly PresenceTracker _tracker;

        private readonly IHubContext<PresenceHub> _presenceHub;

        public MessageHub(IunitOfWork unitOfWork,
        PresenceTracker tracker,
         IMapper mapper,
         IHubContext<PresenceHub> presenceHub)
        {
            _unitOfWork = unitOfWork;
            _tracker = tracker;
            _presenceHub = presenceHub;
            _mapper = mapper;

        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.getUserName(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("updatedGroup", group);

            var message = await _unitOfWork.MessageRepository
            .getMessagesThread(Context.User.getUserName(), otherUser);
            Console.WriteLine("Out Save  otherUser: "+otherUser+" Context.ConnectionId : "+ Context.ConnectionId.ToString() +" groupName : "+groupName);

            if(_unitOfWork.HasChanges()) {
                await _unitOfWork.Complete();
            Console.WriteLine("In Save  otherUser: "+otherUser+" Context.ConnectionId : "+ Context.ConnectionId.ToString() +" groupName : "+groupName);
            }

            await Clients.Caller.SendAsync("ReciveMessageThread", message);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
          var group=  await RemoverFromMessageGroup();
           await Clients.Group(group.Name).SendAsync("updatedGroup",group);
            await base.OnDisconnectedAsync(exception);
        }


        public async Task sendMessage(CreateMessageDTO createMessageDTO)
        {
            var userName = Context.User.getUserName();
            if (userName == createMessageDTO.recipientUserName.ToLower())
                throw new HubException("You Can't Send Message to Yourself");
            var sender = await _unitOfWork.UserRepository.getUserByUserNameAsync(userName);
            var recipient = await _unitOfWork.UserRepository.getUserByUserNameAsync(createMessageDTO.recipientUserName);


            if (recipient == null) throw new HubException("No Found User");
            var message = new Message
            {
                sender = sender,
                recipient = recipient,
                senderUserName = sender.UserName,
                recipientUserName = recipient.UserName,
                content = createMessageDTO.content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _unitOfWork.MessageRepository.getMessageGroup(groupName);
            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                Console.WriteLine("In  group.Connections.Any "+recipient.UserName);
                
                Console.WriteLine("In  group.Connections.Any :: "+group.Connections.ToString());
                message.dateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.getConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.knownAs });
                }
            }
            _unitOfWork.MessageRepository.addMessage(message);

            if (await _unitOfWork.Complete())
            {
                //return Ok(_map.Map<MessageDTO>(message));
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
            // else return BadRequest("Failed Send Message");

        }
        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.getMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.getUserName());
            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);

            }
            group.Connections.Add(connection);
            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to join Group");

        }
        private async Task<Group> RemoverFromMessageGroup()
        {
            var group = await _unitOfWork.MessageRepository.getGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove Group");
        }
        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}