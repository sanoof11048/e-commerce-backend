using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.Services.Users;

namespace Plashoe.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)

        {
            var result = await _userService.Login(loginDTO);
            if (result == null || !string.IsNullOrEmpty(result.Error))
                return BadRequest(result?.Error ?? "Login failed");

            return Ok(result);
        }
    }
}
