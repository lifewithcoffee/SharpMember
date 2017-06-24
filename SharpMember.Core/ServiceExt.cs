using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using SharpMember.Core.Data;
using SharpMember.Core.Data.RepositoryBase;
using SharpMember.Core.Global;
using SharpMember.Global;
using Microsoft.EntityFrameworkCore;

namespace SharpMember.Core
{
    public static class ServiceExt
    {
        static private void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMemberProfileItemRepository, MemberProfileItemRepository>();
            services.AddScoped<IMemberProfileRepository, MemberProfileRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
        
        static private void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IFullMemberSheetReadService, FullMemberSheetReadService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        static public void AddSharpMemberCore(this IServiceCollection services)
        {
            switch (GlobalConfigs.DatabaseType)
            {
                case eDatabaseType.Sqlite:
                    services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>( 
                        options => options.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}")
                    );
                    break;
                case eDatabaseType.SqlServer:
                    // Do nothing here, the relevant work is done in Startup.ConfigureServices().
                    // To avoid adding SqlServer EF dependency in SharpMember.Core project, it's decided not to move the code here.
                    break;
                default:
                    throw new Exception("Unknown database type for DbContext dependency injection");
            }

            services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

            services.AddRepositories();
            services.AddServices();
        }
    }
}
