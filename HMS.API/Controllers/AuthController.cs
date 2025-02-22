using HMS.BL.Helpers;
using HMS.Core.Dtos.AuthDtos;
using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Exceptions.Common;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService _service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterPatience(PatienceCreateDto dto)
        {
            var result = await _service.RegisterAsPatienceAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to Register");
            }
            return Ok("Check your email and confirm!");

        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorCreateDto dto)
        {
            var result = await _service.RegisterAsDoctorAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to Register");
            }
            return Ok("Check your email and confirm!");

        }
        [HttpPost]
        [Authorize(Roles =RoleConstants.Admin)]
        public async Task<IActionResult> ConfirmDoctorAccount(Guid id)
        {
            var result = await _service.ConfirmDoctorAccountAsync(id);
            if (!result)
            {
                return BadRequest("Failed to Confirm");
            }
            return Ok("Account successfully confirmed");
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }
        [HttpGet]
        [Authorize]
        public IActionResult LogOut()
        {
            return Ok( _service.LogOut());
            //The server doesn't store the token, so there's nothing to delete. It processing on the frontend
        }


    }
}
