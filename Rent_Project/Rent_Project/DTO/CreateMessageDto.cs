﻿using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class CreateMessageDto
    {

            public string PlainText { get; set; }
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
        }
    }


