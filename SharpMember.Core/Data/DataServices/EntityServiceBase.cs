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
        readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public EntityServiceBase(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IRepository<T> repo
        )
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public IRepository<T> Repo => _repo;

        public bool Commit()
        {
            return _unitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}
