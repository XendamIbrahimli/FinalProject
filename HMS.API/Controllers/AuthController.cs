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
        public async Task<IActionResult> Register(PatienceCreateDto dto)
        {
            try
            {
                var result=await _service.RegisterAsPatienceAsync(dto);
                if (!result)
                {
                    return BadRequest("Failed to Register");
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
    }
}
