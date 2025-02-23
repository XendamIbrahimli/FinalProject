using HMS.Core.Dtos.PatienceCommentDtos;
using HMS.Core.Dtos.PatienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IPatienceService
    {
        //Create method is in AuthService
        Task<IEnumerable<PatienceGetDto>> GetAll();
        Task<PatienceGetDto> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(PatienceUpdateDto dto);
        Task<bool> DeleteAsync();
    }
}
