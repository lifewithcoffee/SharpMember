using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Services;
using SharpMember.Core.Data;
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

namespace SharpMember.Core
{
    public class RepositoryReader<TEntity>
        : RepositoryRead<TEntity, ApplicationDbContext>
        where TEntity : class
    {
        public RepositoryReader(IUnitOfWork<ApplicationDbContext> unitOfWork)
            : base(unitOfWork)
        { }
    }

    public class RepositoryWriter<TEntity>
        : RepositoryWrite<TEntity, ApplicationDbContext>
        where TEntity : class
    {
        public RepositoryWriter(IUnitOfWork<ApplicationDbContext> unitOfWork)
            : base(unitOfWork)
        { }
    }

    public class Repository<TEntity>
        : Repository<TEntity, ApplicationDbContext>
        where TEntity : class
    {
        public Repository(
            IRepositoryRead<TEntity, ApplicationDbContext> repoReader,
            IRepositoryWrite<TEntity, ApplicationDbContext> repoWriter
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
            bool config_UnitTestConnectionEnabled = Configuration.GetValue<bool>("UnitTestConnectionEnabled");
            switch (GlobalConfigs.DatabaseType)
            {
                case eDatabaseType.Sqlite:
                    services.AddDbContext<ApplicationDbContext>(
                        options => options.UseSqlite($"Filename={DbConsts.SqliteDbFileName}")
                    );
                    break;
                case eDatabaseType.SqlServer:
                    string connectionStringConfig = "DefaultConnection";
                    if (config_UnitTestConnectionEnabled)
                        connectionStringConfig = "UnitTestConnection";

                    services.AddDbContext<ApplicationDbContext>( options =>
                        options.UseSqlServer(
                            Configuration.GetConnectionString(connectionStringConfig), 
                            sqlServerOption => sqlServerOption.MigrationsAssembly("SharpMember.Migrations.SqlServer")
                        ));
                    break;
                case eDatabaseType.Postgres:
                    string postgresConnStr = "PostgresConnection";
                    if (config_UnitTestConnectionEnabled)
                        postgresConnStr = "PostgresConnection_UnitTest";

                    services.AddDbContext<ApplicationDbContext>( options =>
                        options.UseNpgsql(
                            Configuration.GetConnectionString(postgresConnStr), 
                            postgresOption => postgresOption.MigrationsAssembly("SharpMember.Migrations.Postgres")
                        ));

                    // TODO: refactor, ref: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli#using-one-context-type
                    services.AddDbContext<TaskDbContext>(options =>
                        options.UseNpgsql(
                            Configuration.GetConnectionString(postgresConnStr),
                            postgresOption => postgresOption.MigrationsAssembly("SharpMember.Migrations.Postgres")
                        ));
                    break;
                default:
                    throw new Exception("Unknown database type for DbContext dependency injection");
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddRepositoryServices();
            services.AddServices();
            services.AddViewServices();
        }
    }
}
