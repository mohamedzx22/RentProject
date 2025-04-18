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
        public async Task<IActionResult> AddLandlord(String name,int pass,int num,String mail,int role)
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
    }
}
