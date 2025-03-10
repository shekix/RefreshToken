using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshTokenDemo.Models;
using System.Security.Claims;

namespace RefreshTokenDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Validate the user credentials here (e.g., database check)
            if (model.Username == "user" && model.Password == "password") 
            {
                var accessToken = _jwtService.GenerateAccessToken("userId");
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Store the refresh token, such as a database
                
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }


        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
        {
            // Validate refresh token here
            var principal = _jwtService.GetPrincipalFromExpiredToken(model.AccessToken);
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier);

            if (userId == null || model.RefreshToken != "5dee0800-818b-45ec-9b04-be56f2181a84") // Replace with stored refresh token logic
            {
                return Unauthorized();
            }

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(userId.Value);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Store the new refresh token
            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        [Authorize]
        [HttpGet("demo-result")]
        public IActionResult getSomething()
        {
            return Ok(new { result = "returned something" });
        }
    }
}
