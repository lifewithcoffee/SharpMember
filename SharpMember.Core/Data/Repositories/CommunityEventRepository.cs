using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.ActivitySystem;

namespace SharpMember.Core.Data.Repositories
{
    public interface ICommunityEventRepository : IRepositoryBase<ClubEvent, ApplicationDbContext> { }

    public class CommunityEventRepository : RepositoryBase<ClubEvent, ApplicationDbContext>, ICommunityEventRepository
    {
        public CommunityEventRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
