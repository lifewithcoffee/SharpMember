using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using SharpMember.Core.Data;
using SharpMember.Core.Data.RepositoryBase;

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
            services.AddEntityFrameworkSqlite().AddDbContext<SqliteDbContext>();   // NOTE: declared as Transient for multithreading cases
            using (var client = new SqliteDbContext())
            {
                client.Database.EnsureCreated();
            }

            services.AddScoped<IUnitOfWork<SqliteDbContext>, UnitOfWork<SqliteDbContext>>();

            services.AddRepositories();
            services.AddServices();
        }
    }
}
