// Controllers/AddressController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.Services.Addresses;
using System.Security.Claims;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{userId:int}")]
        //[Authorize]
        public async Task<IActionResult> GetUserAddresses(int userId)
        {
            var addresses = await _addressService.GetByUserId(userId);
            return Ok(addresses);
        }

        [HttpGet("address/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _addressService.GetById(id);
            return address != null ? Ok(address) : NotFound("Address not found");
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Add(int id, [FromBody] AddressDTO dto)
        {
            var success = await _addressService.Add(id,dto);
            return success ? Ok("Address added") : BadRequest("Could not add address");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _addressService.Delete(id);
            return success ? Ok("Address deleted") : NotFound("Address not found");
        }

 
    }
}
