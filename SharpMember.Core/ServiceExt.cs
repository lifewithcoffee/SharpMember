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
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using SharpMember.Core.Views.ViewServices.MemberViewServices;
using SharpMember.Core.Views.ViewServices.GroupViewServices;

namespace SharpMember.Core
{
    public static class ServiceExt
    {
        static private void AutoMapperConfiguration()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Member, MemberUpdateVM>();
                cfg.CreateMap<MemberUpdateVM, Member>();
            });
        }

        static private void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberProfileItemRepository, MemberProfileItemRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IMemberProfileItemTemplateRepository, MemberProfileItemTemplateRepository>();
            services.AddScoped<IGroupMemberRelationRepository, GroupMemberRelationRepository>();
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
            services.AddTransient<IMemberCreateViewService, MemberCreateViewService>();
            services.AddTransient<IMemberEditViewService, MemberEditViewService>();

            services.AddTransient<ICommunityIndexViewService, CommunityIndexViewService>();
            services.AddTransient<ICommunityCreateViewService, CommunityCreateViewService>();
            services.AddTransient<ICommunityEditViewService, CommunityEditViewService>();

            services.AddTransient<ICommunityMembersViewService, CommunityMembersViewService>();
            services.AddTransient<ICommunityGroupsViewService, CommunityGroupsViewService>();

            services.AddTransient<IGroupCreateViewService, GroupCreateViewService>();
            services.AddTransient<IGroupEditViewService, GroupEditViewService>();
        }

        static public void AddSharpMemberCore(this IServiceCollection services, IConfiguration Configuration)
        {
            AutoMapperConfiguration();

            switch (GlobalConfigs.DatabaseType)
            {
                case eDatabaseType.Sqlite:
                    services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>( 
                        options => options.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}")
                    );
                    break;
                case eDatabaseType.SqlServer:
                    services.AddDbContext<ApplicationDbContext>(
                        options =>
                        {
                            bool config_UnitTestConnectionEnabled = Configuration.GetValue<bool>("UnitTestConnectionEnabled"); // this setting should be from secrets.json
                            if ( config_UnitTestConnectionEnabled == true)
                            {
                                options.UseSqlServer(Configuration.GetConnectionString("UnitTestConnection"));
                            }
                            else
                            {
                                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                            }
                        }
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
