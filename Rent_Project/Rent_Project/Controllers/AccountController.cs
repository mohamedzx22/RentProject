using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Services;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; 

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IRefreshTokenService _refreshTokenServices;


        public AccountController(AccountService accountService, IRefreshTokenService refreshTokenServices)
        {
            _accountService = accountService;
            _refreshTokenServices = refreshTokenServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            if (result is string  )
                return BadRequest(result);
            return Ok( "Registered Successfully");
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var result = await _accountService.LoginAsync(dto);
            if (result is string)
            {
                return BadRequest(result);
            }
                return Ok("login Successfully"+result);
        }

        [HttpPost("logout")]
         
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync(User);
            return Ok(result);
        }
 
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken)
        {
            var result = await _refreshTokenServices.RefreshTokenAsync(refreshToken.Token);

            if (result == null)

                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }




    }
}
