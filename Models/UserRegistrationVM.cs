using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserRegistrationVM
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Password { get; set; }
    }
}
