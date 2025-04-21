using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;

namespace Rent_Project.Repositories
{
	public class MessageRepository
	{
		private readonly RentAppDbContext _context;

		public MessageRepository(RentAppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
		{
			return await _context.Messages
				.Where(m =>
					(m.SenderId == senderId && m.ReceiverId == receiverId) ||
					(m.SenderId == receiverId && m.ReceiverId == senderId))
				.OrderBy(m => m.date)
				.ToListAsync();
		}

		public async Task<Message> SendMessageAsync(Message message)
		{
			await _context.Messages.AddAsync(message);
			await _context.SaveChangesAsync();
			return message;
		}
	}
}
