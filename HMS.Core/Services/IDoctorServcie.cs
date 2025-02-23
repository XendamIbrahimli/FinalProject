using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IDoctorServcie
    {
        Task<IEnumerable<DoctorGetDto>> GetAll();
        Task<DoctorGetDto> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(DoctorUpdateDto dto);
        Task<bool> DeleteAsync();
    }
}
