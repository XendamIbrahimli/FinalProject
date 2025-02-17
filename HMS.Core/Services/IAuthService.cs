using HMS.Core.Dtos.AuthDtos;
using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsPatienceAsync(PatienceCreateDto dto);
        Task<bool> RegisterAsDoctorAsync(DoctorCreateDto dto);
        Task<bool> ConfirmDoctorAccountAsync(Guid id);
        Task<string> LoginAsync(LoginDto dto);
        string GenerateToken(string userId, string username, string email, string role);
        Task<bool> LogOutAsync();
    }
}
