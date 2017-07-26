﻿using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.ActivitySystem;

namespace SharpMember.Core.Data.Repositories
{
    public interface IGatheringRepository : IRepositoryBase<Gathering, ApplicationDbContext> { }

    public class GatheringRepository : RepositoryBase<Gathering, ApplicationDbContext>, IGatheringRepository
    {
        public GatheringRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}