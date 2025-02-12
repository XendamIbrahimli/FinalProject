using HMS.BL.Enums;
using HMS.BL.Helpers;
using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Exceptions;
using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml; 

namespace HMS.BL.Services
{
    public class AuthService(IPatienceRepository _PatienceRepo, UserManager<User> _userManager,IEmailService _emailService,IDoctorRepository _DoctorRepo, IDepartmentRepository _DepartmentRepo) : IAuthService
    {
        public async Task<bool> ConfirmDoctorAccountAsync(Guid id)
        {
            var data=await _DoctorRepo.GetByIdAsync(id);
            if (data == null)
                throw new NullReferenceException();
            data.IsApproved=true;
            var result=_DoctorRepo.Update(data);
            await _DoctorRepo.SaveAsync();
            return result;
        }

        public async Task<bool> RegisterAsDoctorAsync(DoctorCreateDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            if (await _DoctorRepo.GetAll().AnyAsync(x => x.Fullname == dto.Fullname))
                throw new ExistException("This Fullname already exist");
            if (await _userManager.FindByNameAsync(dto.Email) != null)
                throw new ExistException("This Email already exist");
            if (await _userManager.FindByNameAsync(dto.Username) != null)
                throw new ExistException("This Username already exist");
            if (!await _DepartmentRepo.GetAll().AnyAsync(x => x.Id == dto.DepartmentId))
                throw new NotFoundException("Department not found");


            var filepath = await FileUpload.UploadFileAsync(dto.ImageUrl, "images");


            User user = new User()
            {
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    throw new Exception($"{item.Description}");
                }
            }
            var result1 = await _userManager.AddToRoleAsync(user, nameof(Roles.Doctor));
            if (!result1.Succeeded)
            {
                foreach (var item in result1.Errors)
                {
                    throw new Exception($"{item.Description}");
                }
            }

            Doctor doctor = new Doctor()
            {
                FullInfo=dto.FullInfo,
                Fullname=dto.Fullname,
                LicenseNumber=dto.LicenseNumber,
                DepartmentId=dto.DepartmentId,
                ExperienceYear=dto.ExperienceYear,
                ImageUrl=filepath,
                UserId=user.Id
            }; 

            bool f=await _DoctorRepo.AddAsync(doctor);
            await _DoctorRepo.SaveAsync();

            //Send Verification code

            await _emailService.SendConfimationCodeAsync(user.Email);

            return f;
        }

        public async Task<bool> RegisterAsPatienceAsync(PatienceCreateDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            if (await _PatienceRepo.GetAll().AnyAsync(x => x.Fullname == dto.Fullname))
                throw new ExistException("This Fullname already exist");
            if (await _userManager.FindByNameAsync(dto.Email) != null)
                throw new ExistException("This Email already exist");
            if (await _userManager.FindByNameAsync(dto.Username)!=null)
                throw new ExistException("This Username already exist");


            User user=new User()
            {
                UserName=dto.Username,
                Email=dto.Email,
                PhoneNumber=dto.PhoneNumber
            };

            var result=await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    throw new Exception($"{item.Description}");
                }
            }
            var result1=await _userManager.AddToRoleAsync(user,nameof(Roles.Patience));
            if (!result1.Succeeded)
            {
                foreach (var item in result1.Errors)
                {
                    throw new Exception($"{item.Description}");
                }
            }

            Patience patience = new Patience()
            {
                Fullname=dto.Fullname,
                Gender=dto.Gender,
                DateOfBirth=dto.DateOfBirth,
                UserId=user.Id
            };

            bool f=await _PatienceRepo.AddAsync(patience);
            await _PatienceRepo.SaveAsync();

            //Send Verification code

            await _emailService.SendConfimationCodeAsync(user.Email);

            return f;
        }

    }
}
