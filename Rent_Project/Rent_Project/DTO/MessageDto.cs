// DTO/MessageDto.cs
using System;

namespace Rent_Project.DTO
{
    public class MessageDto
    {
        public string PlainText { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
    }
}
