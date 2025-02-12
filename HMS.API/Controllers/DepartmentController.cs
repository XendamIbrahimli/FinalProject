using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Exceptions;
using HMS.Core.Exceptions.Common;
using HMS.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentControlle(IDepartmentService _service) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]DepartmentCreateDto dto)
        {
            bool result = await _service.CreateAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to create department");
            }
            return Ok("Department Created successfully");
        }
        //[HttpPost("create-range")]
        //public async Task<IActionResult> CreateRange([FromForm]List<DepartmentCreateDto> list)
        //{
        //        bool result = await _service.CreateRangeAsync(list);
        //        if (!result)
        //        {
        //            return BadRequest("Failed to create department");
        //        }
        //        return Ok("Departments Created successfully");

        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());

        }

        [HttpGet("ById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _service.GetByIdAsync(id));

        }
        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromForm] DepartmentUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result)
            {
                return BadRequest("Failed to update department");
            }
            return Ok("Department Updated successfully");

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete department");
            }
            return Ok("Department Deleted successfully");
        }
    }
}
