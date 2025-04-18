﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly RentAppDbContext _db;
        public PostController (RentAppDbContext db)
        {
            _db = db;
        }

        [HttpPost("view-post/{id}")]
        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.Number_of_viewers += 1;
            await _db.SaveChangesAsync();

            return Ok(new { viewers = post.Number_of_viewers });
        }


        [HttpGet]
        public async Task<IActionResult> GetAccseptedPost()
        {
            var Posts = _db.Posts
                .Where(p=> p.Accsepted_Status == 1)
                .Select(L => new
                  {
                      L.Title,
                      L.Description,
                      L.location,
                      L.Price,
                      L.rental_status,
                      L.Number_of_viewers,
                      L.Landlord_name,
                      L.images,
                  })
                .ToList();
            return Ok(Posts);
        }

        [HttpGet("landlord/{landlordId}")]
        public async Task<IActionResult> GetPostsByLandlordId(int landlordId)
        {
            var posts = await _db.Posts
               .Where(p => p.Landlord_id == landlordId)
               .Select(L => new
                  {
                     L.Title,
                     L.Description,
                     L.location,
                     L.Price,
                     L.rental_status,
                     L.Number_of_viewers,
                     L.Landlord_name,
                     L.images,
                     L.Accsepted_Status,
                   })
               .ToListAsync();

            if (posts == null || posts.Count == 0)
                return NotFound("No posts found for this landlord.");

            return Ok(posts);
        }

        


    }
}
