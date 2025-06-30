using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class APIValidationResponse :ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public APIValidationResponse(IEnumerable<string> errors=null, int? statusCode =400  ) : base(statusCode)
        {
            Errors = errors ?? new List<string>();
        }
        // public int statusCode { get; set; }
        //public string messages { get; set; }
        //public bool IsSuccess { get; set; }
    }
}
