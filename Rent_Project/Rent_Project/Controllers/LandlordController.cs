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
        [HttpPost]
        public async Task<IActionResult> AddLandlord(String name, String pass, String num, String mail, int role)
        {
            User Landlord = new User();
            {
                Landlord.name = name;
                Landlord.password = pass;
                Landlord.number = num;
                Landlord.email = mail;
                Landlord.role = role;
            }
            await _db.Users.AddAsync(Landlord);
            _db.SaveChanges();
            return Ok(Landlord);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLandlord(User Landlord)
        {
          var name= await _db.Users.SingleOrDefaultAsync(x=> x.id == Landlord.id);
            if (name == null)
            {
                return NotFound();
            }
            name.name = Landlord.name;
            name.password = Landlord.password;
            name.number = Landlord.number;
            name.email = Landlord.email;
            name.role = Landlord.role;
            _db.SaveChanges();
            return Ok(name);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLandlord(int id)
        {
            var landlord = await _db.Users.SingleOrDefaultAsync(x=>x.id==id);
            if (landlord == null)
            {
                return NotFound();
            }
            _db.Users.Remove(landlord);
            _db.SaveChanges();
            return Ok(landlord);
        }

        [HttpGet("pending-landlords")]
        public async Task<IActionResult> GetPendingLandlords()
        {
            var pendingLandlords = await _db.Users
                .Where(u => u.role == 2 && u.Landlord_Status == 0)
                .Select(u => new {
                    u.id,
                    u.name,
                    u.email,
                    u.number
                })
                .ToListAsync();

            return Ok(pendingLandlords);
        }


        [HttpPost("approve-landlord/{id}")]
        public async Task<IActionResult> ApproveLandlord(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null || user.role != 2)
                return NotFound("User not found or not a landlord.");

            user.Landlord_Status = 1;
            await _db.SaveChangesAsync();

            return Ok("Landlord approved successfully.");
        }

        [HttpPost("reject-landlord/{id}")]
        public async Task<IActionResult> RejectLandlord(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null || user.role != 2)
                return NotFound("User not found or not a landlord.");

            user.Landlord_Status = 2; 
            await _db.SaveChangesAsync();

            return Ok("Landlord rejected successfully.");
        }

        [HttpDelete("delete-rejected-landlords")]
        public async Task<IActionResult> DeleteRejectedLandlords()
        {
            var rejectedLandlords = await _db.Users
                .Where(u => u.role == 2 && u.Landlord_Status == 2)
                .ToListAsync();

            if (!rejectedLandlords.Any())
                return NotFound("No rejected landlords found.");

            _db.Users.RemoveRange(rejectedLandlords);
            await _db.SaveChangesAsync();

            return Ok($"{rejectedLandlords.Count} rejected landlord(s) deleted.");
        }



    }
}
