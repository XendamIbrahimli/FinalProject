using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.EmailServiceDtos
{
    public class EmailDto
    {
        [MaxLength(128),EmailAddress]
        public string Email { get; set; } = null!;

        public string code { get; set; } = null!;
    }
}
