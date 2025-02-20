using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Dtos.PatienceCommentDtos;
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

namespace HMS.BL.Services
{
    public class PatienceCommentService(IHttpContextAccessor _http, UserManager<User> _userManager,IPatienceCommentRepository _commentRepo, IPatienceRepository _patienceRepo) : IPatienceCommentService
    {
        public async Task<bool> CreateAsync(CreatePatienceComment dto)
        {
            var UserId=GetUserIdFromToken();

            User? user = await _userManager.Users.Include(x=>x.Patience).FirstOrDefaultAsync(x=>x.Id==UserId);

            if (user == null)
                throw new NotFoundException("User doesn't exist.");

            PatienceComment patienceComment = new PatienceComment()
            {
                PatienceFullname = user.Patience!.Fullname,
                PatienceId = user.Patience.Id,
                Comment = dto.Comment
            };
            var result=await _commentRepo.AddAsync(patienceComment);
            await _commentRepo.SaveAsync();
            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var comment=await _commentRepo.GetByIdAsync(id);
            if(comment == null)
                throw new NotFoundException("Comment doesn't exist");

            var UserId = GetUserIdFromToken();
            var user = await _userManager.Users.Where(x => x.Id == UserId).Include(x=>x.Patience).FirstOrDefaultAsync();
            if (user == null)
                throw new NullReferenceException();

            if (user.Patience!.Id != comment.PatienceId)
                throw new NotPermissionException("You cann't delete this comment, because it isn't your comment");

            comment.IsDeleted = true;
            await _commentRepo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<GetPatienceCommentDto>> GetAll()
        {
            var comments = await _commentRepo.GetAll().Include(x=>x.Patience).ToListAsync();

            if (!comments.Any() || comments == null)
                throw new NotFoundException("Comment doesn't exist");

            var commentsDtos = comments.Select(x => new GetPatienceCommentDto
            {
                PatienceFullname=x.Patience!.Fullname,
                Comment=x.Comment
            }).ToList();

            return commentsDtos;
        }

        public async Task<GetPatienceCommentDto> GetByIdAsync(Guid id)
        {
            var Patience=await _patienceRepo.GetAll().Include(x=>x.PatienceComment).FirstOrDefaultAsync(x=>x.Id==id);
            if (Patience == null)
                throw new NotFoundException("Patience doesn't exist");

            if (Patience.PatienceComment == null)
                throw new NullReferenceException("This Patience's comment isn't exist ");

            return new GetPatienceCommentDto
            {
                PatienceFullname = Patience.Fullname,
                Comment = Patience.PatienceComment.Comment
            };
        }

        public async Task<bool> UpdateAsync(UpdatePatienceCommentDto dto)
        {
            var userId = GetUserIdFromToken();
            User? user=await _userManager.Users.Include(x=>x.Patience).FirstOrDefaultAsync(x=>x.Id==userId);
            if (user == null)
                throw new NullReferenceException();

            Guid PatienceId=user.Patience!.Id;

            var comment = await _commentRepo.GetWhere(x => x.PatienceId == PatienceId).FirstOrDefaultAsync();
            if (comment == null)
                throw new NotFoundException("This patience doesn't have exist comment");

            comment.UpdatedTime=DateTime.UtcNow;
            comment.Comment=dto.NewComment;

            bool result=_commentRepo.Update(comment);
            await _commentRepo.SaveAsync();
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
