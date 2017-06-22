using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberProfileRepository : IRepositoryBase<MemberProfile, SqliteDbContext>
    {
    }

    public class MemberProfileRepository : RepositoryBase<MemberProfile, SqliteDbContext>, IMemberProfileRepository
    {
        public MemberProfileRepository(IUnitOfWork<SqliteDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
