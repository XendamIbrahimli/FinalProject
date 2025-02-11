using HMS.Core.Dtos.EmailServiceDtos;
using HMS.Core.Exceptions.Common;
using HMS.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService _service) : ControllerBase
    {
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailDto dto)
        {
            try
            {
                var result =await _service.ConfirmEmailCodeAsync(dto);
                if (!result)
                {
                    return BadRequest("Wrong code");
                }
                return Ok("Registered successfully");
            }
            catch (Exception ex)
            {
                if (ex is IBaseException Bex)
                {
                    return StatusCode(Bex.StatusCode, new
                    {
                        Message = Bex.ErrorMessage,
                        StatusCode = Bex.StatusCode
                    });
                }
                return BadRequest(new
                {
                    ex.Message
                });
            }
        }
        [HttpPost("sendNewCode")]
        public async Task<IActionResult> SendNewCode(string email)
        {
            if(email.Contains('@') && email.Length <= 128)
            {
                await _service.SendConfimationCodeAsync(email);
                return Ok("Message was send");
            }
            return BadRequest("Enter email correctly!");
        }
    }
}
