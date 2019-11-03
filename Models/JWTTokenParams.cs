using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public  class JWTTokenParams
    {
        public const string Issuer = "Me";
        public const string Audience = "MySiteUsers";
        public const string Key = "12345678900987654321";
    }
}
