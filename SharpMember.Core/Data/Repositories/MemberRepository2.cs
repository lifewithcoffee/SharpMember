using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberRepository2 : IRepositoryBase<MemberProfile,ApplicationDbContext>
    {
        MemberProfile GetByMemberNumber(int memberNumber);
        void ImportFromExcel();
        List<MemberProfile> GetByProfileItem(string itemName, string itemValue);
    }

    public class MemberRepository2 : RepositoryBase<MemberProfile, ApplicationDbContext>, IMemberRepository2
    {
        IMemberProfileItemRepository _memberProfileItemRepository;

        public MemberRepository2(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger logger,
            IMemberProfileItemRepository memberProfileItemRepository) : base(unitOfWork, logger)
        {
            this._memberProfileItemRepository = memberProfileItemRepository;
        }

        public MemberProfile GetByMemberNumber(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public List<MemberProfile> GetByProfileItem(string itemName, string itemValue)
        {
            return _memberProfileItemRepository.GetMany(i => i.ItemName == itemName && i.ItemValue == itemValue).Select(i => i.MemberProfile).ToList();
        }

        public void ImportFromExcel()
        {
            throw new NotImplementedException();
        }
    }
}
