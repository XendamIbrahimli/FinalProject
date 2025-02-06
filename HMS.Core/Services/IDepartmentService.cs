using HMS.Core.Dtos.DepartmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IDepartmentService
    {
        Task<bool> CreateAsync(DepartmentCreateDto dto);
    }
}
