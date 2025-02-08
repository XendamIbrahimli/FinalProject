using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IDepartmentService
    {
        Task<bool> CreateAsync(DepartmentCreateDto dto);
        //Task<bool> CreateRangeAsync(List<DepartmentCreateDto> list); then look again 
        Task<IEnumerable<DepartmentGetDto>> GetAll();
        Task<DepartmentGetDto> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, DepartmentUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        
    }
}
