// Controllers/MessageController.cs
using Microsoft.AspNetCore.Mvc;
using Rent_Project.DTO;
using Rent_Project.Services;
using System.Threading.Tasks;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        public MessageController(MessageService messageService)
            => _messageService = messageService;

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDto dto)
        {
            await _messageService.SendMessageAsync(dto);
            return Ok("Message sent successfully");
        }

        [HttpGet("chat/{senderId}/{receiverId}")]
        public async Task<IActionResult> GetChat(int senderId, int receiverId)
        {
            var messages = await _messageService.GetMessagesBetweenUsers(senderId, receiverId);
            return Ok(messages);
        }
    }
}
