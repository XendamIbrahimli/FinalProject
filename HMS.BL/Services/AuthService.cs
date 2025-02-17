using HMS.BL.Enums;
using HMS.BL.Helpers;
using HMS.Core.Dtos.AuthDtos;
using HMS.Core.Dtos.DoctorDtos;
using HMS.Core.Dtos.PatienceDtos;
using HMS.Core.Exceptions;
using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml; 

namespace HMS.BL.Services
{
    public class AuthService(IPatienceRepository _PatienceRepo, UserManager<User> _userManager,IEmailService _emailService,IDoctorRepository _DoctorRepo, IDepartmentRepository _DepartmentRepo, IConfiguration _config) : IAuthService
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

        public string GenerateToken(string userId, string username, string email, string role)
        {
            JwtOptions jwtOptions = new JwtOptions();
            jwtOptions.Audience = _config.GetSection("JwtOptions")["Audience"]!;
            jwtOptions.Issuer = _config.GetSection("JwtOptions")["Issuer"]!;
            jwtOptions.Secret = _config.GetSection("JwtOptions")["Secret"]!;

            SymmetricSecurityKey key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            SigningCredentials cred=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);


            List<Claim> claims = 
            [
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            ];

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore:DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            User? user;
            if(dto.UserNameOrEmail.Contains('@'))
                user=await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            else
                user=await _userManager.FindByNameAsync(dto.UserNameOrEmail);

            if (user == null || !await _userManager.CheckPasswordAsync(user,dto.Password))
                throw new NullReferenceException("Email/Username or Password is wrong");

            if (user.Doctor?.IsApproved == false)
                throw new NotPermissionException ("Your account isn't approved by admin");

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateToken(user.Id, user.UserName!, user.Email!, roles[0]);
            return token;

        }

        public Task<bool> LogOutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsDoctorAsync(DoctorCreateDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            if (await _DoctorRepo.GetAll().AnyAsync(x => x.Fullname == dto.Fullname))
                throw new ExistException("This Fullname already exist");
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
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
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
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
