using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.DepartmentDtos
{
    public class DepartmentGetDto
    {
        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;
        [MaxLength(256)]
        public string Description { get; set; } = null!; 
    }
}
