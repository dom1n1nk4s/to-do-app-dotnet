using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace to_do_app_dotnet.Models
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public DbSet<Entry> Entries { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}