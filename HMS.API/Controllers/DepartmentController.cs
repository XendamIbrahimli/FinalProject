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
            try
            {
                bool result=await _service.CreateAsync(dto);
                if(!result)
                {
                    return BadRequest("Failed to create department");
                }
                return Ok("Department Created successfully");

            }catch (Exception ex)
            {
                if(ex is IBaseException Bex)
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
        //[HttpPost("create-range")]
        //public async Task<IActionResult> CreateRange([FromForm]List<DepartmentCreateDto> list)
        //{
        //    try
        //    {
        //        bool result = await _service.CreateRangeAsync(list);
        //        if (!result)
        //        {
        //            return BadRequest("Failed to create department");
        //        }
        //        return Ok("Departments Created successfully");

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is IBaseException Bex)
        //        {
        //            return StatusCode(Bex.StatusCode, new
        //            {
        //                Message = Bex.ErrorMessage,
        //                StatusCode = Bex.StatusCode
        //            });
        //        }
        //        return BadRequest(new
        //        {
        //            ex.Message
        //        });

        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAll());

            }catch (Exception ex)
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

        [HttpGet("ById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                return Ok(await _service.GetByIdAsync(id));

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
        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromForm] DepartmentUpdateDto dto)
        {
            try
            {
                var result=await _service.UpdateAsync(id, dto);
                if (!result)
                {
                    return BadRequest("Failed to update department");
                }
                return Ok("Department Updated successfully");

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
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    return BadRequest("Failed to delete department");
                }
                return Ok("Department Deleted successfully");

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
