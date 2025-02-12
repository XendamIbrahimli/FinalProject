using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Exceptions.Common;
using HMS.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _service) : ControllerBase
    {
        [HttpPost("AsPatience")]
        public async Task<IActionResult> RegisterPatience(PatienceCreateDto dto)
        {
            var result = await _service.RegisterAsPatienceAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to Register");
            }
            return Ok("Check your email and confirm!");

        }

        [HttpPost("AsDoctor")]
        public async Task<IActionResult> RegisterDoctor(DoctorCreateDto dto)
        {
            var result = await _service.RegisterAsDoctorAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to Register");
            }
            return Ok("Check your email and confirm!");

        }
        [HttpPost("ConfirmDoctorAccount")]
        public async Task<IActionResult> ConfirmDoctorAccount(Guid id)
        {
            var result = await _service.ConfirmDoctorAccountAsync(id);
            if (!result)
            {
                return BadRequest("Failed to Confirm");
            }
            return Ok("Account successfully confirmed");
        }

    }
}
