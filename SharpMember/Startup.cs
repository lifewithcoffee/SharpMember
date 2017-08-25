using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Models;
using SharpMember.Services;
using SharpMember.Core;
using SharpMember.Core.Definitions;
using SharpMember.Authorization;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Data;
using Microsoft.Extensions.Logging;

namespace SharpMember
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSharpMemberCore(Configuration);
            services.AddMvc();
            services.AddAuthorization(options => {
                options.AddPolicy(PolicyName.RequireRoleOf_CommunityOwner,
                    policy =>
                    {
                        policy.Requirements.Add(new CommunityRoleRequirement(RoleName.CommunityOwner));
                        policy.RequireAuthenticatedUser();
                    }
                );
                
                options.AddPolicy(PolicyName.RequireRoleOf_CommunityManager,
                    policy =>
                    {
                        policy.Requirements.Add(new CommunityRoleRequirement(RoleName.CommunityManager));
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
            services.AddTransient<IAuthorizationHandler, CommunityRoleHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "short_community_url",
                    template: "c/{action=Index}/{id?}",
                    defaults: new { Controller = "Communities" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            serviceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
        }
    }
}
