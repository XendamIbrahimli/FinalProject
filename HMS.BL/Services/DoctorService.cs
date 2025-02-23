using HMS.BL.Helpers;
using HMS.Core.Dtos.DoctorDtos;
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

namespace HMS.BL.Services
{
    public class DoctorService(IDoctorRepository _repo, IHttpContextAccessor _http, IEmailService _emailService, UserManager<User> _userManager) : IDoctorServcie
    {
        public async Task<bool> DeleteAsync()
        {
            var userId = GetUserIdFromToken();
            var doctor = await _repo.GetWhere(x => x.UserId == userId).Include(x => x.User).FirstOrDefaultAsync();
            if (doctor == null)
                throw new NullReferenceException();
            doctor!.IsDeleted = true;
            await _repo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<DoctorGetDto>> GetAll()
        {
            var datas = await _repo.GetAll().Include(x => x.User).Include(x => x.Department).ToListAsync();

            if (!datas.Any() || datas == null)
                throw new NotFoundException("Doctor doesn't exist");

            var datasDtos = datas.Select(x => new DoctorGetDto
            {
                Fullname = x.Fullname,
                ImageUrl = x.ImageUrl,
                Username = x.User!.UserName!,
                Email = x.User.Email!,
                PhoneNumber = x.User.PhoneNumber!,
                FullInfo=x.FullInfo,
                LicenseNumber=x.LicenseNumber,
                ExperienceYear=x.ExperienceYear,
                DepartmentName=x.Department!.Name,
                IsApproved=x.IsApproved
            }).ToList();

            return datasDtos;
        }

        public async Task<DoctorGetDto> GetByIdAsync(Guid id)
        {
            var doctor = await _repo.GetWhere(x => x.Id == id).Include(x => x.User).Include(x => x.Department).FirstOrDefaultAsync();
            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            var dto = new DoctorGetDto()
            {
                Fullname = doctor.Fullname,
                ImageUrl = doctor.ImageUrl,
                Username = doctor.User!.UserName!,
                Email = doctor.User.Email!,
                PhoneNumber = doctor.User.PhoneNumber!,
                FullInfo = doctor.FullInfo,
                LicenseNumber = doctor.LicenseNumber,
                ExperienceYear = doctor.ExperienceYear,
                DepartmentName = doctor.Department!.Name,
                IsApproved = doctor.IsApproved
            };
            return dto;
        }

        public async Task<bool> UpdateAsync(DoctorUpdateDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();

            var UserId = GetUserIdFromToken();
            var User = await _userManager.FindByIdAsync(UserId);
            var doctor = await _repo.GetWhere(x => x.UserId == UserId).Include(x => x.User).Include(x=>x.Department).FirstOrDefaultAsync();


            if (doctor == null)
                throw new NotFoundException("Doctor not found");
            if (!string.IsNullOrEmpty(dto.Fullname))
                doctor.Fullname = dto.Fullname;

            if (dto.ImageUrl != null)
            {
                string OldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doctor.ImageUrl);
                if (File.Exists(OldfilePath))
                {
                    File.Delete(OldfilePath);
                }
                doctor.ImageUrl = await FileUpload.UploadFileAsync(dto.ImageUrl, "images");
            }
            if (!string.IsNullOrEmpty(dto.Email))
            {
                await _emailService.SendConfimationCodeAsync(dto.Email);
                User!.Email = dto.Email;
                User!.EmailConfirmed = false;
            }
            if (!string.IsNullOrEmpty(dto.FullInfo))
                doctor.FullInfo = dto.FullInfo;

            if (!string.IsNullOrEmpty(dto.LicenseNumber))
                doctor.LicenseNumber = dto.LicenseNumber;

            if (dto.ExperienceYear!=0)
                doctor.ExperienceYear = dto.ExperienceYear;

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                User!.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrEmpty(dto.Password) && dto.Password == dto.RePassword)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(User);
                var result1 = await _userManager.ResetPasswordAsync(User, resetToken, dto.Password);
                if (!result1.Succeeded)
                    return false;
            }

            doctor.UpdatedTime = DateTime.Now;
            var result = _repo.Update(doctor);
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
