using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class DBEntitiesContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DBEntitiesContext(DbContextOptions<DBEntitiesContext> options): base(options)
        {

        }
    }
}
