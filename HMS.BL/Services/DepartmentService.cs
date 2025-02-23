using HMS.BL.Helpers;
using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Exceptions;
using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Services
{
    public class DepartmentService(IDepartmentRepository _repo) : IDepartmentService
    {
        public async Task<bool> CreateAsync(DepartmentCreateDto dto)
        {
            if (await _repo.GetAll().AnyAsync(x => x.Name == dto.Name))
                throw new ExistException($"{dto.Name} is exist");

            var filePath = await FileUpload.UploadFileAsync(dto.Icon, "icons");


            Department department = new Department()
            {
                Description = dto.Description,
                Name = dto.Name,
                Icon=filePath
            };
            bool f= await _repo.AddAsync(department);
            await _repo.SaveAsync();
            return f;
        }

        //public async Task<bool> CreateRangeAsync(List<DepartmentCreateDto> list)
        //{
        //    var departments = new List<Department>();
        //    foreach (var dto in list)
        //    {
        //        if (await _repo.GetAll().AnyAsync(x => x.Name == dto.Name))
        //            throw new ExistException($"{dto.Name} is exist");

        //        var filePath = await FileUpload.UploadFileAsync(dto.Icon, "icons");


        //        departments.Add(new Department()
        //        {
        //            Description = dto.Description,
        //            Name = dto.Name,
        //            Icon = filePath
        //        });
        //    }

        //    bool result=await _repo.AddRangeAsync(departments);
        //    await _repo.SaveAsync();
        //    return result;
        //}

        public async Task<IEnumerable<DepartmentGetDto>> GetAll()
        {
            var departments = await _repo.GetAll().ToListAsync();

            if (!departments.Any() || departments == null)
                throw new NotFoundException("Department doesn't exist");

            var departmentsDtos = departments.Select(x => new DepartmentGetDto
            {
                Name = x.Name,
                Icon = x.Icon,
                Description = x.Description
            }).ToList();

            return departmentsDtos;
        }

        public async Task<DepartmentGetDto> GetByIdAsync(Guid DepartmentId)
        {
            var department=await _repo.GetByIdAsync(DepartmentId);
            if (department == null)
                throw new NotFoundException("department not found");

            var dto = new DepartmentGetDto()
            {
                Name = department.Name,
                Icon = department.Icon,
                Description = department.Description
            };
            return dto;
        }

        public async Task<bool> UpdateAsync(Guid id, DepartmentUpdateDto dto)
        {
            if (await _repo.GetAll().AnyAsync(x => x.Name == dto.Name))
                throw new ExistException($"{dto.Name} is exist");

            Department department=await _repo.GetByIdAsync(id);
            if (department == null)
                throw new NotFoundException("department not found");

            var newFilePath = await FileUpload.UploadFileAsync(dto.Icon, "icons");

            string OldfilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",department.Icon);
            if (File.Exists(OldfilePath))
            {
                File.Delete(OldfilePath);
            }

            department.Name = dto.Name;
            department.Description = dto.Description;
            department.Icon=newFilePath;
            department.UpdatedTime=DateTime.UtcNow;

            bool result=_repo.Update(department);
            await _repo.SaveAsync();
            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Department department = await _repo.GetByIdAsync(id);
            if (department == null)
                throw new NotFoundException("department not found");

            department.IsDeleted = true;
            await _repo.SaveAsync();
            return true;
        }
    }
}
