using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.PatienceCommentDtos
{
    public class CreatePatienceComment
    {
        [MaxLength(256)]
        public string Comment { get; set; } = null!;
    }
}
