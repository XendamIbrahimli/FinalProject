using HMS.Core.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Exceptions
{
    public class FileUploadException:Exception,IBaseException
    {
        public int StatusCode=>StatusCodes.Status406NotAcceptable;
        public string ErrorMessage { get; }

        public FileUploadException(string message):base(message) 
        {
            ErrorMessage = message;
        }
    }
}
