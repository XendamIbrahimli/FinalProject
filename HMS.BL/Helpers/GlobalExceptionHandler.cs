using HMS.Core.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Helpers
{
    public class GlobalExceptionHandler:IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            if (ex is IBaseException Bex)  
            {
                context.Result = new ObjectResult(new { Message = Bex.ErrorMessage, StatusCode = Bex.StatusCode })
                {
                    StatusCode = Bex.StatusCode
                };
            }
            else
            {
                context.Result = new BadRequestObjectResult(new { Message = ex.Message });
            }

            context.ExceptionHandled = true;
        }
    }
}
