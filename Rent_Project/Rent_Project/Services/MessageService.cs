// Services/MessageService.cs
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_Project.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepo;

        public MessageService(IMessageRepository messageRepo)
            => _messageRepo = messageRepo;

        public async Task SendMessageAsync(CreateMessageDto dto)
        {
            var entity = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                text = EncryptionHelper.Encrypt(dto.PlainText),
                date = DateTime.UtcNow
            };
            await _messageRepo.AddAsync(entity);
        }

        public async Task<List<MessageDto>> GetMessagesBetweenUsers(int senderId, int receiverId)
        {
            var chat = await _messageRepo.GetChatAsync(senderId, receiverId);
            return chat.Select(m => new MessageDto
            {
                PlainText = EncryptionHelper.Decrypt(m.text),
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                SentAt = m.date
            }).ToList();
        }
    }
}
