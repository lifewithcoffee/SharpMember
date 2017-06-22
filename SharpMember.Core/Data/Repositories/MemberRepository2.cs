using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberRepository2 : IRepositoryBase<Member2,SqliteDbContext>
    {
        Member2 GetByMemberNumber(int memberNumber);
        void ImportFromExcel();
        List<Member2> GetByProfileItem(string itemName, string itemValue);
    }

    public class MemberRepository2 : RepositoryBase<Member2, SqliteDbContext>, IMemberRepository2
    {
        IMemberProfileItemRepository _memberProfileItemRepository;

        public MemberRepository2(
            IUnitOfWork<SqliteDbContext> unitOfWork,
            ILogger logger,
            IMemberProfileItemRepository memberProfileItemRepository) : base(unitOfWork, logger)
        {
            this._memberProfileItemRepository = memberProfileItemRepository;
        }

        public Member2 GetByMemberNumber(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public List<Member2> GetByProfileItem(string itemName, string itemValue)
        {
            return _memberProfileItemRepository.GetMany(i => i.ItemName == itemName && i.ItemValue == itemValue).Select(i => i.Member2).ToList();
        }

        public void ImportFromExcel()
        {
            throw new NotImplementedException();
        }
    }
}
