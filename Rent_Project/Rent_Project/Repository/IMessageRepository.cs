using Rent_Project.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Rent_Project.Repository
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<List<Message>> GetChatAsync(int senderId, int receiverId);
    }
}
