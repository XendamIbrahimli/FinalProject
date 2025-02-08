using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Exceptions.Common
{
    public interface IBaseException
    {
        int StatusCode {  get; }
        string ErrorMessage {  get; }
    }
}
