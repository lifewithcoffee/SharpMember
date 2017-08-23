using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using SharpMember.Core.Data;
using SharpMember.Core.Data.RepositoryBase;
using SharpMember.Core.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharpMember.Core.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewServices;
using SharpMember.Core.Views.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace SharpMember.Core
{
    public static class ServiceExt
    {
        static private void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberProfileItemRepository, MemberProfileItemRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IMemberProfileItemTemplateRepository, MemberProfileItemTemplateRepository>();
            services.AddScoped<IMemberGroupRoleRelationRepository, MemberGroupRoleRelationRepository>();
        }
        
        static private void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IFullMemberPageReader, FullMemberPageReader>();
            services.AddTransient<IZjuaaaMemberExcelFileReadService, ZjuaaaMemberExcelFileReadService>();
            services.AddTransient<IAssociatedMemberPageReader, AssociatedMemberPageReader>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
        }

        static private void AddViewServices(this IServiceCollection services)
        {
            services.AddTransient<IMemberIndexViewService, MemberIndexViewService>();
            services.AddTransient<IMemberCreateViewService, MemberCreateViewService>();
            services.AddTransient<IMemberEditViewService, MemberEditViewService>();

            services.AddTransient<IOrganizationIndexViewService, OrganizationIndexViewService>();
            services.AddTransient<IOrganizationCreateViewService, OrganizationCreateViewService>();
            services.AddTransient<IOrganizationEditViewService, OrganizationEditViewService>();
        }

        static public void AddSharpMemberCore(this IServiceCollection services, IConfiguration Configuration)
        {
            switch (GlobalConfigs.DatabaseType)
            {
                case eDatabaseType.Sqlite:
                    services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>( 
                        options => options.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}")
                    );
                    break;
                case eDatabaseType.SqlServer:
                    services.AddDbContext<ApplicationDbContext>(
                        options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );
                    break;
                default:
                    throw new Exception("Unknown database type for DbContext dependency injection");
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

            services.AddRepositories();
            services.AddServices();
            services.AddViewServices();
        }
    }
}
