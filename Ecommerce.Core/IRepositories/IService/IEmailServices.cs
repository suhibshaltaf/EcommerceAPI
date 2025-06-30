using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.IRepositories.IService
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string toemail,string subject,string message);
    }
}
