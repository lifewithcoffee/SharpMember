using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data;
using SharpMember.Core;
using SharpMember.Authorization;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Global;

namespace SharpMember
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSharpMemberCore(Configuration);
            services.AddMvc();
            services.AddAuthorization(options => {
                options.AddPolicy(PolicyName.RequireRoleOf_OrganizationOwner,
                    policy =>
                    {
                        policy.Requirements.Add(new OrganizationRoleRequirement(RoleName.OrganizationOwner));
                        policy.RequireAuthenticatedUser();
                    }
                );
                
                options.AddPolicy(PolicyName.RequireRoleOf_OrganizationManager,
                    policy =>
                    {
                        policy.Requirements.Add(new OrganizationRoleRequirement(RoleName.OrganizationManager));
                        policy.RequireAuthenticatedUser();
                    }
                );

                options.AddPolicy(PolicyName.RequireRoleOf_GroupOwner,
                    policy =>
                    {
                        policy.Requirements.Add(new GroupRoleRequirement(RoleName.GroupOwner));
                        policy.RequireAuthenticatedUser();
                    }
                );

                options.AddPolicy(PolicyName.RequireRoleOf_GroupManager,
                    policy =>
                    {
                        policy.Requirements.Add(new GroupRoleRequirement(RoleName.GroupManager));
                        policy.RequireAuthenticatedUser();
                    }
                );
            });
            services.AddTransient<IAuthorizationHandler, GroupRoleHandler>();
            services.AddTransient<IAuthorizationHandler, OrganizationRoleHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "short_organization_url",
                    template: "o/{action=Index}/{id?}",
                    defaults: new { Controller = "Organizations" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            serviceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
        }
    }
}
