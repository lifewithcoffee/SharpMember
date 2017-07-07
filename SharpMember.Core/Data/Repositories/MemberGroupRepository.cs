using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberGroupRepository : IRepositoryBase<MemberGroup, ApplicationDbContext> { }

    public class MemberGroupRepository : RepositoryBase<MemberGroup, ApplicationDbContext>, IMemberGroupRepository
    {
        public MemberGroupRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
