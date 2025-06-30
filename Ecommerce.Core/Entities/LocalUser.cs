using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Ecommerce.Core.Entities
{
    public class LocalUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adrress { get; set; }
        public virtual ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();
    }

}
