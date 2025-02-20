using HMS.BL.Helpers;
using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Dtos.PatienceCommentDtos;
using HMS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatienceCommentController(IPatienceCommentService _service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles =RoleConstants.Patience)]
        public async Task<IActionResult> Create([FromForm] CreatePatienceComment dto)
        {
            bool result = await _service.CreateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to create Comment");
            }
            return Ok("Comment Created successfully");
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminAndModerator)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminAndModerator)]
        public async Task<IActionResult> GetByPatienceId(Guid PatienceId)
        {
            return Ok(await _service.GetByIdAsync(PatienceId));
        }
        [HttpPut]
        [Authorize(Roles = RoleConstants.Patience)]
        public async Task<IActionResult> Update([FromForm]UpdatePatienceCommentDto dto)
        {
            bool result = await _service.UpdateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to update Comment");
            }
            return Ok("Comment Updated successfully");
        }
        [HttpDelete]
        [Authorize(Roles = RoleConstants.Patience)]
        public async Task<IActionResult> Delete(Guid CommentId)
        {
            return Ok(await _service.DeleteAsync(CommentId));
        }
    }
}
