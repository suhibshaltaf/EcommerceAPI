﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.DTO
{
    public class RestPasswordDTO
    {
        public string Email { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }

        public string Token { get; set; }

    }
}
