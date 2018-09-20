using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using SharpMember.Core.Data;
using NetCoreUtils.Database;
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
        static private bool automapper_initialized = false;
        static private readonly object locker = new object();
        static private void AutoMapperConfiguration()
        {
            /* Add thread locking to prevent this exception in xunit for the latest AutoMapper:
             * Mapper already initialized. You must call Initialize once per application domain/process
             */
            lock (locker)
            {
                if (!automapper_initialized)
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<Member, MemberUpdateVM>();
                        cfg.CreateMap<MemberUpdateVM, Member>();

                        cfg.CreateMap<GroupUpdateVM, Group>();
                        cfg.CreateMap<Group, GroupUpdateVM>();

                        cfg.CreateMap<CommunityUpdateVM, Community>();
                        cfg.CreateMap<Community, CommunityUpdateVM>();
                    });
                    automapper_initialized = true;
                }
            }
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
            services.AddTransient<ICommunityService, CommunityService>();
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
                    services.AddDbContext<ApplicationDbContext>(
                        options => options.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}")
                    );
                    break;
                case eDatabaseType.SqlServer:
                    services.AddDbContext<ApplicationDbContext>(
                        options =>
                        {
                            bool config_UnitTestConnectionEnabled = Configuration.GetValue<bool>("UnitTestConnectionEnabled");
                            if ( config_UnitTestConnectionEnabled == true)
                                options.UseSqlServer(Configuration.GetConnectionString("UnitTestConnection"));
                            else
                                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
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
