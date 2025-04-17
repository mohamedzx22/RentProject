using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        public LandlordController(RentAppDbContext db) {
            _db = db;
        }

        private readonly RentAppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetLandlords()
        {
        var landlords = await _db.Users
        .Where(u => u.role == 2)
        .Select(u => new {
            u.id,
            u.name,
            u.email,
            u.number
        }).ToListAsync();

            return Ok(landlords);
        }
    }
}
