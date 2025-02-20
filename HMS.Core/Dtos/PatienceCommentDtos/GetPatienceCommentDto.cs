using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.PatienceCommentDtos
{
    public class GetPatienceCommentDto
    {
        [MaxLength(256)]
        public string Comment { get; set; } = null!;
        [MaxLength(32)]
        public string PatienceFullname { get; set; } = null!;
    }
}
