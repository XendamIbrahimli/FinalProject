using HMS.BL.Helpers;
using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Exceptions;
using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace HMS.BL.Services
{
    public class PatienceService(IPatienceRepository _repo, IHttpContextAccessor _http, IEmailService _emailService,UserManager<User> _userManager) : IPatienceService
    {
        public async Task<bool> DeleteAsync()
        {
            var userId = GetUserIdFromToken();
            var patient = await _repo.GetWhere(x => x.UserId == userId).Include(x => x.User).FirstOrDefaultAsync();
            if (patient == null)
                throw new NullReferenceException();
            patient!.IsDeleted=true;
            await _repo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PatienceGetDto>> GetAll()
        {
            var datas = await _repo.GetAll().Include(x=>x.User).ToListAsync();

            if (!datas.Any() || datas == null)
                throw new NotFoundException("Patience doesn't exist");

            var datasDtos = datas.Select(x => new PatienceGetDto
            {
                Fullname=x.Fullname,
                ImageUrl=x.ImageUrl,
                Gender=x.Gender,
                DateOfBirth=x.DateOfBirth,
                Username=x.User!.UserName!,
                Email=x.User.Email!,
                PhoneNumber=x.User.PhoneNumber!

            }).ToList();

            return datasDtos;
        }

        public async Task<PatienceGetDto> GetByIdAsync(Guid PatienceId)
        {
            var patience = await _repo.GetWhere(x=>x.Id==PatienceId).Include(x=>x.User).FirstOrDefaultAsync();
            if (patience == null)
                throw new NotFoundException("Patience not found");

            var dto = new PatienceGetDto()
            {
                Fullname = patience.Fullname,
                ImageUrl = patience.ImageUrl,
                Gender = patience.Gender,
                DateOfBirth = patience.DateOfBirth,
                Username = patience.User!.UserName!,
                Email = patience.User.Email!,
                PhoneNumber = patience.User.PhoneNumber!
            };
            return dto;
        }

        public async Task<bool> UpdateAsync(PatienceUpdateDto? dto)
        {
            if (dto == null)
                throw new NullReferenceException();

            var UserId = GetUserIdFromToken();
            var User=await _userManager.FindByIdAsync(UserId);
            var Patient = await _repo.GetWhere(x => x.UserId == UserId).Include(x => x.User).FirstOrDefaultAsync();


            if (Patient == null)
                throw new NotFoundException("Patience not found");
            if(!string.IsNullOrEmpty(dto.Fullname))
                Patient.Fullname=dto.Fullname;

            if(dto.ImageUrl != null)
            {
                string OldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Patient.ImageUrl);
                if (File.Exists(OldfilePath))
                {
                    File.Delete(OldfilePath);
                }
                Patient.ImageUrl = await FileUpload.UploadFileAsync(dto.ImageUrl, "PatienceImages");
            }
            if (!string.IsNullOrEmpty(dto.Email))
            {
                await _emailService.SendConfimationCodeAsync(dto.Email);
                User!.Email=dto.Email;
                User!.EmailConfirmed = false;
            }
            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                User!.PhoneNumber=dto.PhoneNumber;

            if (!string.IsNullOrEmpty(dto.Password)&& dto.Password==dto.RePassword)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(User);
                var result1 = await _userManager.ResetPasswordAsync(User, resetToken, dto.Password);
                if (!result1.Succeeded)
                    return false;
            }    

            Patient.UpdatedTime = DateTime.Now;
            var result= _repo.Update(Patient);
            await _repo.SaveAsync();

            return result;
        }
        public string GetUserIdFromToken()
        {
            var claimsIdentity = (ClaimsIdentity)_http.HttpContext.User.Identity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
    }
}
