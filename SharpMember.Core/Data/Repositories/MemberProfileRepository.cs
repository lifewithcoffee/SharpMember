using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberProfileRepository : IRepositoryBase<MemberProfile, ApplicationDbContext>
    {
    }

    public class MemberProfileRepository : RepositoryBase<MemberProfile, ApplicationDbContext>, IMemberProfileRepository
    {
        public MemberProfileRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
