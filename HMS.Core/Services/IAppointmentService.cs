using HMS.Core.Dtos.AppointmentDtos;
using HMS.Core.Dtos.DepartmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IAppointmentService
    {
        string GetUserIdFromToken();
        Task<string> CreateAppointmentAsync(CreateAppointmentDto dto);
        Task<bool> ConfirmAppointmentAsync(Guid id, string action);
        Task<IEnumerable<GetAppointmentDto>> GetAllAsync();
        Task<IEnumerable<GetAppointmentDto>> GetByDoctorIdAsync(Guid id);
        Task<bool> UpdateAsync(UpdateAppointmentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
