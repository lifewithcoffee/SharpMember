using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DataServices
{
    public class EntityServiceBase<T> : ICommittable
        where T : class
    {
        readonly IRepository<T> _repo;

        public EntityServiceBase(IRepository<T> repo)
        {
            _repo = repo;
        }

        public IRepository<T> Repo => _repo;

        public bool Commit()
        {
            return _repo.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _repo.CommitAsync();
        }
    }
}
