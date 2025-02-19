using HMS.BL.Helpers;
using HMS.Core.Dtos.AppointmentDtos;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentController(IAppointmentService _appService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = RoleConstants.Patience)]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            return Ok(await _appService.CreateAppointmentAsync(dto));
        }
        [HttpPost]
        [Authorize(Roles =RoleConstants.Doctor)]
        public async Task<IActionResult> ConfirmAppointment(Guid AppointmentId,string action)
        {
            return Ok(await _appService.ConfirmAppointmentAsync(AppointmentId, action));
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminAndModerator)]
        public async Task<IActionResult> GetAllApointments()
        {
            return Ok(await _appService.GetAllAsync());
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.Doctor)]
        public async Task<IActionResult> GetByDoctorId(Guid id)
        {
            return Ok(await _appService.GetByDoctorIdAsync(id));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]UpdateAppointmentDto dto)
        {
            bool result = await _appService.UpdateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to update appointment");
            }
            return Ok("Appointment Updated successfully");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            return Ok(await _appService.DeleteAsync(id));
        }

    }
}
