using SharpMember.Data.Models;
using SharpMember.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SharpMember.Data.Repositories
{
    public interface IUserRepository : IRepositoryBase<ApplicationUser, SqliteDbContext>
    {
        //bool ValidateLastChanged(ClaimsPrincipal userPrincipal, string lastChanged); //see: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie
    }

    public class UserRepository : RepositoryBase<ApplicationUser, SqliteDbContext>, IUserRepository
    {
        public UserRepository(IUnitOfWork<SqliteDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
