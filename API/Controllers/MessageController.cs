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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        // private readonly IMessageRepository _unitOfWork.MessageRepository;
        // private readonly IUserRepository _unitOfWork.UserRepository;
        private readonly IMapper _map;
        private readonly IunitOfWork _unitOfWork;

        public MessageController(IunitOfWork unitOfWork,IMapper map)
        {
            _unitOfWork = unitOfWork;
            _map = map;
      
        }
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> createMessage(CreateMessageDTO createMessageDTO)
        {
            var userName = User.getUserName();
            if (userName == createMessageDTO.recipientUserName.ToLower())
                return BadRequest("You Can't Send Message to Yourself");
                Console.WriteLine("Crete Message 🎈");
            var sender = await _unitOfWork.UserRepository.getUserByUserNameAsync(userName);
            var recipient = await _unitOfWork.UserRepository.getUserByUserNameAsync(createMessageDTO.recipientUserName);


            if (recipient == null) return NotFound();
            var message = new Message
            {
                sender = sender,
                recipient = recipient,
                senderUserName = sender.UserName,
                recipientUserName = recipient.UserName,
                content = createMessageDTO.content
            };
            _unitOfWork.MessageRepository.addMessage(message);

            if (await _unitOfWork.Complete()) return Ok(_map.Map<MessageDTO>(message));
            else return BadRequest("Failed Send Message");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> getMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.userName = User.getUserName();
            var messages = await _unitOfWork.MessageRepository.getMessagesForUser(messageParams);
            Response.addPaginationHeader(messages.currentPage,
            messages.pageSize, messages.totalCount, messages.totalPages);
            return messages;

        }


        // [HttpGet("thread/{userName}")]
        // public async Task<ActionResult<IEnumerable<MessageDTO>>> getMessageThread(string userName)
        // {
        //     var currentUserUserName = User.getUserName();
        //     return Ok(await _unitOfWork.MessageRepository.getMessagesThread(currentUserUserName, userName));
        // }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteMessage(int id)
        {
            var userName = User.getUserName();
            var message=await _unitOfWork.MessageRepository.GetMessage(id);
            if(message.sender.UserName!=userName && message.recipient.UserName!=userName) return Unauthorized();
            if(message.sender.UserName==userName)message.senderDeleted=true;
            if(message.recipient.UserName==userName)message.recipientDeleted=true;
            if(message.senderDeleted && message.recipientDeleted) 
            _unitOfWork.MessageRepository.deleteMessage(message);

            if(await _unitOfWork.Complete())return Ok();

            return BadRequest("Problem Deleting the Message");
            
        }

    }
}