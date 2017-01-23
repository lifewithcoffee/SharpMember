using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Data.Models;

namespace SharpMember.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GlobalSettings> GlobalSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure unique constraint for UserAdditionalInfo.MemberNumber
            // from: http://ef.readthedocs.io/en/latest/modeling/relational/unique-constraints.html
            builder.Entity<ApplicationUser>().HasAlternateKey(i => i.MemberNumber).HasName("AlternateKey_MemberNumber");
        }
    }
}
