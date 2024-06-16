using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Definitions;
using System.Data.SqlClient;
using SharpMember.Utils;
using SharpMember.Core.Data.Models.Meeting;
using SharpMember.Core.Data.Models.Project;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;

namespace SharpMember.Core.Data.DbContexts;

// Use default DB schema
public class GlobalContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<GlobalSettings> GlobalSettings { get; set; }

    /**
     * The following constructor will cause mvc scaffolding to throw out an exception of
     *      Unable to resolve service for type 'System.String'
     * when choose "MVC Controller with read/write actions"
     */
    //public ApplicationDbContext(string connectionString) : base(GetOptionsFromConnectionString(connectionString)) { }

    public GlobalContext(DbContextOptions<GlobalContext> options) : base(options)
    {
        if (GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
            Database.EnsureCreated();
    }
}
