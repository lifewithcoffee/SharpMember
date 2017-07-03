using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberManagement;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberProfileItemRepository : IRepositoryBase<ProfileItem, ApplicationDbContext> { }

    public class MemberProfileItemRepository : RepositoryBase<ProfileItem, ApplicationDbContext>, IMemberProfileItemRepository
    {
        public MemberProfileItemRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
