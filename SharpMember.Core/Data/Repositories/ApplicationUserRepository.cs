using SharpMember.Core.Data.Models;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SharpMember.Core.Data.Repositories
{
    public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser, ApplicationDbContext>
    {
        //bool ValidateLastChanged(ClaimsPrincipal userPrincipal, string lastChanged); //see: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser, ApplicationDbContext>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<ApplicationUserRepository> logger) : base(unitOfWork, logger) { }
    }
}
