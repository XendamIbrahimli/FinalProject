using HMS.Core.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Services
{
    public class ExistException:Exception,IBaseException
    {
        public int StatusCode => StatusCodes.Status406NotAcceptable;
        public string ErrorMessage { get; }

        public ExistException(string message): base(message) 
        {
            ErrorMessage = message;
        }
    }
}
