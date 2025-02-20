using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.PatienceCommentDtos
{
    public class UpdatePatienceCommentDto
    {
        [MaxLength(256)]
        public string NewComment { get; set; } = null!;
    }
}
