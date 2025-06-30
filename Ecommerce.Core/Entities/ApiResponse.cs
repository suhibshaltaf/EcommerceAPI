using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class ApiResponse
    {
         public int? StatusCode {  get; set; }
        public bool IsSuccess { get; set; }
        public string? Messages { get; set; }
        public Object? Result { get; set; }

            public ApiResponse(int? statusCode=null,string? messages=null,object? result = null)
        {
            StatusCode=statusCode;
            Messages=messages ?? getMessageForStatusCode(statusCode);
            Result=result;
            IsSuccess = statusCode >= 200 && statusCode <= 300;
            

        }
        private string? getMessageForStatusCode( int? statusCode)
        {
            return statusCode switch
            {
                200 => "successfully",
                201=> "created  successfully",
                400 => "Bad Request",
                404 => "Not Found",
                500 => "Internal server error",
                _ => null

            };
        }
    }
}
