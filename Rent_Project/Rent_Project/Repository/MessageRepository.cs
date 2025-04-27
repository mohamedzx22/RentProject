using Rent_Project.Model;
using Microsoft.EntityFrameworkCore;

namespace Rent_Project.Repository
{
    public class MessageRepository :IMessageRepository
    {
        
            private readonly RentAppDbContext _context;
            public MessageRepository(RentAppDbContext context) => _context = context;

            public async Task AddAsync(Message message)
            {
                await _context.Masseges.AddAsync(message);
                await _context.SaveChangesAsync();
            }

            public async Task<List<Message>> GetChatAsync(int senderId, int receiverId)
            {
                return await _context.Masseges
                    .Where(m =>
                        (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                        (m.SenderId == receiverId && m.ReceiverId == senderId))
                    .OrderBy(m => m.date)
                    .ToListAsync();
            }
        }
    
}
