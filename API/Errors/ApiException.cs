using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null,string details=null) : base(statusCode, message)
        {
        Deatils=details;
        }
        public string Deatils{get;set;}
    }
}