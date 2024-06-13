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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SharpMember.Controllers.APIs;

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
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSharpMemberCore(Configuration);
            services.AddMvc(option => option.EnableEndpointRouting = false );
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            // what to validate
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,

                            // setup validation data
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UsersController.test_security_key))
                        };
                    });

            services.AddAuthorization(options => {
                options.AddPolicy(PolicyNames.RequireRoleOf_CommunityOwner,
                    policy =>
                    {
                        policy.Requirements.Add(new CommunityRoleRequirement(RoleNames.CommunityOwner));
                        policy.RequireAuthenticatedUser();
                    }
                );
                
                options.AddPolicy(PolicyNames.RequireRoleOf_CommunityManager,
                    policy =>
                    {
                        policy.Requirements.Add(new CommunityRoleRequirement(RoleNames.CommunityManager));
                        policy.RequireAuthenticatedUser();
                    }
                );

                options.AddPolicy(PolicyNames.RequireRoleOf_GroupOwner,
                    policy =>
                    {
                        policy.Requirements.Add(new GroupRoleRequirement(RoleNames.GroupOwner));
                        policy.RequireAuthenticatedUser();
                    }
                );

                options.AddPolicy(PolicyNames.RequireRoleOf_GroupManager,
                    policy =>
                    {
                        policy.Requirements.Add(new GroupRoleRequirement(RoleNames.GroupManager));
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
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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
                    name: "short_community_url_index",
                    template: "c",
                    defaults: new { Controller = "Communities", Action = "Index" });

                routes.MapRoute(
                    name: "short_community_url_edit",
                    template: "c/{id}",
                    defaults: new { Controller = "Communities", Action = "Edit" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
