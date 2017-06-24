using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberProfileItemRepository : IRepositoryBase<MemberProfileItem, ApplicationDbContext> { }

    public class MemberProfileItemRepository : RepositoryBase<MemberProfileItem, ApplicationDbContext>, IMemberProfileItemRepository
    {
        public MemberProfileItemRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
