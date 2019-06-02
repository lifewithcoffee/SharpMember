using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SharpMember.Core.Definitions;
using NetCoreUtils.Database;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface ICommunityRepository
    {
        Community Add(string name);
        void CancelMember(string appUserId);
        IRepositoryBase<Community> Repo { get; }
    }

    public class CommunityRepository : ICommunityRepository
    {
        IRepositoryBase<Community> _repo;
        public CommunityRepository(IRepositoryBase<Community> repo)
        {
            _repo = repo;
        }

        public IRepositoryBase<Community> Repo { get { return _repo; } }

        public Community Add(string name)
        {
            return _repo.Add(new Community { Name = name });
        }

        public void CancelMember(string appUserId)
        {
            throw new NotImplementedException();
        }
    }
}
