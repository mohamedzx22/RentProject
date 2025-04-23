﻿namespace Rent_Project.Model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }   
        public DateTime ExpiryDate { get; set; }  
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public User? User { get; set; }
    }
}
