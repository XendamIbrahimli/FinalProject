using HMS.BL.Helpers;
using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Repositories;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class DoctorController(IDoctorServcie _service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminModeratorDoctor)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());

        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminModeratorDoctor)]
        public async Task<IActionResult> GetById(Guid DoctorID)
        {
            return Ok(await _service.GetByIdAsync(DoctorID));

        }
        [HttpPut]
        [Authorize(Roles = RoleConstants.Doctor)]
        public async Task<IActionResult> Update(DoctorUpdateDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to update Doctor");
            }
            return Ok("Doctor Updated successfully");
        }
        [HttpDelete]
        [Authorize(Roles = RoleConstants.Doctor)]
        public async Task<IActionResult> Delete()
        {
            return Ok(await _service.DeleteAsync());
        }
    }
}
