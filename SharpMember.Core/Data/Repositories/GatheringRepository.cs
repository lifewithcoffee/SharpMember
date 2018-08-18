using SharpMember.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.ActivitySystem;
using NetCoreUtils.Database;

namespace SharpMember.Core.Data.Repositories
{
    public interface IGatheringRepository : IRepositoryBase<Gathering, ApplicationDbContext> { }

    public class GatheringRepository : RepositoryBase<Gathering, ApplicationDbContext>, IGatheringRepository
    {
        public GatheringRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<GatheringRepository> logger) : base(unitOfWork, logger)
        {
        }
    }
}
