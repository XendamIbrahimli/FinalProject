using HMS.BL.Helpers;
using HMS.Core.Dtos.AppointmentDtos;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService _appService) : ControllerBase
    {
        [HttpPost("Create-Appointment")]
        [Authorize(Roles = RoleConstants.Patience)]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            return Ok(await _appService.CreateAppointmentAsync(dto));
        }
        [HttpPost("Confirm-Appointment")]
        [Authorize(Roles =RoleConstants.Doctor)]
        public async Task<IActionResult> ConfirmAppointment(Guid AppointmentId,string action)
        {
            return Ok(await _appService.ConfirmAppointmentAsync(AppointmentId, action));
        }
    }
}
