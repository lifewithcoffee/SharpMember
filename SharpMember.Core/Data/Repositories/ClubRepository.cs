using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.EventManagement;

namespace SharpMember.Core.Data.Repositories
{
    public interface IClubRepository : IRepositoryBase<Club, ApplicationDbContext> { }

    public class ClubRepository : RepositoryBase<Club, ApplicationDbContext>, IClubRepository
    {
        public ClubRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
