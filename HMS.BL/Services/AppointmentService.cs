using HMS.BL.Enums;
using HMS.Core.Dtos.AppointmentDtos;
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
    public class AppointmentService(IAppointmentRepository _AppRepo, IDoctorRepository _DocRepo, IHttpContextAccessor _http, UserManager<User> _userManager) : IAppointmentService
    {
        public async Task<bool> ConfirmAppointmentAsync(Guid AppintmentId, string action)
        {
            var userId = GetUserIdFromToken();
            var user = await _userManager.Users.Include(x=>x.Doctor).FirstOrDefaultAsync(x=>x.Id==userId);

            var appointment=await _AppRepo.GetByIdAsync(AppintmentId);
            if (appointment==null)
                throw new NotFoundException("Appointment doesn't exist");
            
            if(user.Doctor.Id!=appointment.DoctorId)
                throw new AnauthorizedException("You are not authorized to perform this action.");
            
            switch (action.ToLower())
            {
                case "confirm":
                    if (appointment.Status == nameof(AppointmentStatus.Waiting))
                    {
                        appointment.Status=nameof(AppointmentStatus.Confirmed);
                        _AppRepo.Update(appointment);
                        await _AppRepo.SaveAsync();
                    }
                    break;
                case "cancel":
                    if (appointment.Status == nameof(AppointmentStatus.Waiting))
                    {
                        appointment.Status = nameof(AppointmentStatus.Canceled);
                        _AppRepo.Update(appointment);
                        await _AppRepo.SaveAsync();
                    }
                    break;
            }
            return true;
        }

        public async Task<string> CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            if (dto == null)
                throw new NullReferenceException();
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
                throw new AnauthorizedException("User is not authenticated.");
            
            var doctor=await _DocRepo.GetWhere(x=>x.Fullname==dto.DoctorName).FirstOrDefaultAsync(); 
            if(doctor==null)
                throw new NotFoundException("This doctor name doesn't exist");

            var user=await _userManager.Users.Include(x=>x.Patience).FirstOrDefaultAsync(x=>x.Id==userId);
            var appointment = new Appointment
            {
                PatienceId = user.Patience.Id,
                DoctorId = doctor.Id,
                Status = nameof(AppointmentStatus.Waiting),
                DateTime = dto.DateTime
            };

            await _AppRepo.AddAsync(appointment);
            await _AppRepo.SaveAsync();
            return "Appointment created successfully. Now call the doctor number(you can get number from our website) for confirm appointment! After appointment confiremd, you can go to the hospital.";

        }

        public string GetUserIdFromToken()
        {
            var claimsIdentity = (ClaimsIdentity)_http.HttpContext.User.Identity;
            var userIdClaim=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
    }
}
