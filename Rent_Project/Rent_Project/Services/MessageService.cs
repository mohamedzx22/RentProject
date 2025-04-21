using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;

public class MessageService
{
    private readonly RentAppDbContext _context;

    public MessageService(RentAppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessageAsync(CreateMessageDto dto)
    {
        var message = new Message
        {
            text = dto.text,
            date = DateTime.Now,
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId
        };

        _context.Messeges.Add(message);
        await _context.SaveChangesAsync();
    }


    public async Task<List<MessageDto>> GetMessagesBetweenUsers(int senderId, int receiverId)
    {
        var messages = await _context.Messeges
            .Where(m =>
                (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                (m.SenderId == receiverId && m.ReceiverId == senderId))
            .OrderBy(m => m.date)
            .Select(m => new MessageDto
            {
                Text = m.text,
                Date = m.date
            })
            .ToListAsync();

        return messages;
    }
}
