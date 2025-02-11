using HMS.BL.Enums;
using HMS.Core.Dtos.PatienceDtos;
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

namespace HMS.BL.Services
{
    public class AuthService(IPatienceRepository _PatienceRepo, UserManager<User> _userManager,IEmailService _emailService) : IAuthService
    {
        public async Task<bool> RegisterAsPatienceAsync(PatienceCreateDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            if (await _PatienceRepo.GetAll().AnyAsync(x => x.Fullname == dto.Fullname))
                throw new ExistException("This Fullname already exist");
            if(await _userManager.FindByNameAsync(dto.Username)!=null)
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
