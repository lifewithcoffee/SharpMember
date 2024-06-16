using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using NetCoreUtils.Database;
using SharpMember.Core.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using SharpMember.Core.Views.ViewServices.MemberViewServices;
using SharpMember.Core.Views.ViewServices.GroupViewServices;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using SharpMember.Core.Data.DbContexts;

namespace SharpMember.Core
{
    public class RepositoryReader<TEntity>
        : RepositoryRead<TEntity, GlobalContext>
        where TEntity : class
    {
        public RepositoryReader(IUnitOfWork<GlobalContext> unitOfWork)
            : base(unitOfWork)
        { }
    }

    public class RepositoryWriter<TEntity>
        : RepositoryWrite<TEntity, GlobalContext>
        where TEntity : class
    {
        public RepositoryWriter(IUnitOfWork<GlobalContext> unitOfWork)
            : base(unitOfWork)
        { }
    }

    public class Repository<TEntity>
        : Repository<TEntity, GlobalContext>
        where TEntity : class
    {
        public Repository(
            IRepositoryRead<TEntity, GlobalContext> repoReader,
            IRepositoryWrite<TEntity, GlobalContext> repoWriter
        ) : base(repoReader, repoWriter)
        { }
    }

    public static class ServiceExt
    {
        static private void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddRepositories();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepositoryRead<>), typeof(RepositoryReader<>));
            services.AddScoped(typeof(IRepositoryWrite<>), typeof(RepositoryWriter<>));

            //services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberProfileItemService, MemberProfileItemService>();
            services.AddScoped<IMemberService, MemberService>();
            //services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IMemberProfileItemTemplateService, MemberProfileItemTemplateService>();
            //services.AddScoped<IGroupMemberRelationRepository, GroupMemberRelationRepository>();
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
            services.AddTransient<IMemberCreateHandler, MemberCreateHandler>();
            services.AddTransient<IMemberEditHandler, MemberEditHandler>();

            services.AddTransient<ICommunityIndexHandler, CommunityIndexHandler>();
            services.AddTransient<ICommunityCreateHandler, CommunityCreateHandler>();
            services.AddTransient<ICommunityEditHandler, CommunityEditHandler>();

            services.AddTransient<ICommunityMembersHandler, CommunityMembersHandler>();
            services.AddTransient<ICommunityGroupsHandler, CommunityGroupsHandler>();

            services.AddTransient<IGroupCreateHandler, GroupCreateHandler>();
            services.AddTransient<IGroupEditHandler, GroupEditHandler>();
            services.AddTransient<IGroupAddMemberHandler, GroupAddMemberHandler>();
        }

        static public void AddSharpMemberCore(this IServiceCollection services, IConfiguration Configuration)
        {
            bool unitTestConnectionEnabled = Configuration.GetValue<bool>("UnitTestConnectionEnabled");
            string connStr, assembly;
            switch (GlobalConfigs.DatabaseType)
            {
                case eDatabaseType.Sqlite:
                    services.AddDbContext<GlobalContext>( o => o.UseSqlite($"Filename={DbConsts.SqliteDbFileName}") );
                    services.AddDbContext<ProjectContext>( o => o.UseSqlite($"Filename={DbConsts.SqliteDbFileName}") );
                    services.AddDbContext<MemberContext>( o => o.UseSqlite($"Filename={DbConsts.SqliteDbFileName}") );
                    break;
                case eDatabaseType.SqlServer:
                    connStr = Configuration.GetConnectionString(unitTestConnectionEnabled ? "UnitTestConnection": "DefaultConnection");
                    assembly = "SharpMember.Migrations.SqlServer";

                    services.AddDbContext<GlobalContext>( o1 => o1.UseSqlServer( connStr, o2 => o2.MigrationsAssembly(assembly)));
                    services.AddDbContext<ProjectContext>( o1 => o1.UseSqlServer( connStr, o2 => o2.MigrationsAssembly(assembly)));
                    services.AddDbContext<MemberContext>( o1 => o1.UseSqlServer( connStr, o2 => o2.MigrationsAssembly(assembly)));
                    break;
                case eDatabaseType.Postgres:
                    connStr = Configuration.GetConnectionString(unitTestConnectionEnabled ? "PostgresConnection_UnitTest": "PostgresConnection");
                    assembly = "SharpMember.Migrations.Postgres";

                    services.AddDbContext<GlobalContext>( o1 => o1.UseNpgsql(connStr, o2 => o2.MigrationsAssembly(assembly) ));
                    services.AddDbContext<ProjectContext>( o1 => o1.UseNpgsql(connStr, o2 => o2.MigrationsAssembly(assembly) ));
                    services.AddDbContext<MemberContext>( o1 => o1.UseNpgsql(connStr, o2 => o2.MigrationsAssembly(assembly) ));
                    break;
                default:
                    throw new Exception("Unknown database type for DbContext dependency injection");
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<GlobalContext>()
                .AddDefaultTokenProviders();

            services.AddRepositoryServices();
            services.AddServices();
            services.AddViewServices();
        }
    }
}
