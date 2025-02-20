using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Dtos.PatienceCommentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IPatienceCommentService
    {
        Task<bool> CreateAsync(CreatePatienceComment dto);
        Task<IEnumerable<GetPatienceCommentDto>> GetAll();
        Task<GetPatienceCommentDto> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(UpdatePatienceCommentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
