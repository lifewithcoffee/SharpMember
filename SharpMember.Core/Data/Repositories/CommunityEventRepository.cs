using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories
{
    public interface ICommunityEventRepository : IRepositoryBase<CommunityEvent, ApplicationDbContext> { }

    public class CommunityEventRepository : RepositoryBase<CommunityEvent, ApplicationDbContext>, ICommunityEventRepository
    {
        public CommunityEventRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
