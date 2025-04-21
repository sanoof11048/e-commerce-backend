using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.Services.Users;

namespace Plashoe.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                return Ok(await _userService.Register(registerDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            return result ? Ok("User deleted") : NotFound("User not found");
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("toggle-block/{id}")]
        public async Task<IActionResult> ToggleBlockUser(int id)
        {
            var response = await _userService.ToggleBlockUser(id);

            return StatusCode(response.StatusCode, response);
        }

    }
}
