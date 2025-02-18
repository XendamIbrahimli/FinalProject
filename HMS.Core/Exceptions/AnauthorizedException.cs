using HMS.Core.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Exceptions
{
    public class AnauthorizedException : Exception, IBaseException
    {
        public int StatusCode => StatusCodes.Status401Unauthorized;
        public string ErrorMessage { get; }
        public AnauthorizedException(string message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}
