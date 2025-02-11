using HMS.BL.Helpers;
using HMS.Core.Dtos.EmailServiceDtos;
using HMS.Core.Models;
using HMS.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Services
{
    public class EmailService(IOptions<SmtpOptions> _opts,IMemoryCache _cache, UserManager<User> _userManager):IEmailService
    {
        readonly SmtpOptions smtp=_opts.Value;

        public async Task SendEmailAsync(string toEmail,string subject, string body)
        {
            var smtpClient = new SmtpClient(smtp.Server, smtp.Port);
            smtpClient.Credentials=new NetworkCredential(smtp.Username,smtp.Password);
            smtpClient.EnableSsl = true;
            var mailMessage=new MailMessage()
            {
                From=new MailAddress(smtp.Username),
                Subject=subject,
                Body=body,
                IsBodyHtml=true
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendConfimationCodeAsync(string email)
        {
            var random = new Random();
            string code = random.Next(100000, 999999).ToString();

            _cache.Set(email, code, TimeSpan.FromMinutes(10));

            string subject = "Your Email Confimation Code";
            string body = $"Your confimation code is: {code}. Please enter this code in your application to confirm your email.";

            await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> ConfirmEmailCodeAsync(EmailDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            if(_cache.TryGetValue(dto.Email, out string? storedCode)) 
            {
                if (storedCode == dto.code)
                {
                    var user=await _userManager.FindByEmailAsync(dto.Email);
                    if (user == null)
                        throw new NullReferenceException("Email didn't find");
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);

                    _cache.Remove(dto.Email);
                    return true;
                }
            }
            return false;
        }
    }
}
