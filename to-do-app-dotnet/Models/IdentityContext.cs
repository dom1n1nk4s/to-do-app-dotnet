using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace to_do_app_dotnet.Models
{
    public interface IIdentityContext
    {
        DbSet<Entry> Entries { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    [ExcludeFromCodeCoverageAttribute]
    public class IdentityContext : IdentityDbContext<User>, IIdentityContext
    {
        public virtual DbSet<Entry> Entries { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}