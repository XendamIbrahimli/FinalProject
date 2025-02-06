using HMS.Core.Dtos.DepartmentDtos;
using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.Core.Services;
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

            Department department = new Department()
            {
                Description = dto.Description,
                Name = dto.Name,
            };

            return await _repo.AddAsync(department); 
        }
    }
}
