using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VueStoreApi.DTOs;
using VueStoreApi.Services;

namespace VueStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register(DTOs.RegisterRequest request)
        {
            var result = await authService.RegisterAsync(request);
            if (result is null)
                return Conflict(new { message = "An account with this email already exists." });

            return Ok(new
            {
                success = true,     
                message = "Registration successful",
                data = result
            });
        }
            
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(DTOs.LoginRequest request)
        {
            var result = await authService.LoginAsync(request);

            if(result is null)
                return Unauthorized(new { message = "Invalid email or password." });


            // Set the JWT token in an HttpOnly cookie
            Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            });



            return Ok(new
                {
                    success = true,
                    message = "Login successful",
                data = new
                {
                    result.UserId,
                    result.Name,
                    result.Email,
                    result.ExpiresAtUtc
                }
            });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/"
                });

            return Ok(new
            {
                success = true,
                message = "Logout successful"
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }
    }
}



