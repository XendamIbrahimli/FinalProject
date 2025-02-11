using HMS.Core.Dtos.EmailServiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email,string body,string subject);
        Task SendConfimationCodeAsync(string email);
        Task<bool> ConfirmEmailCodeAsync(EmailDto dto);
    }
}
