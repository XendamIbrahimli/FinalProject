using HMS.BL.Helpers;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatienceController(IPatienceService _service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles =RoleConstants.AdminAndModerator)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());

        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminAndModerator)]
        public async Task<IActionResult> GetById(Guid PatienceId)
        {
            return Ok(await _service.GetByIdAsync(PatienceId));

        }
        [HttpPut]
        [Authorize(Roles =RoleConstants.Patience)]
        public async Task<IActionResult> Update(PatienceUpdateDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to update Patient");
            }
            return Ok("Patient Updated successfully");
        }
        [HttpDelete]
        [Authorize(Roles = RoleConstants.Patience)]
        public async Task<IActionResult> Delete()
        {
            return Ok(await _service.DeleteAsync());
        }
    }
}
