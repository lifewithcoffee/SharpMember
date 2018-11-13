using SharpMember.Core.Data.Models;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Definitions;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemRepository : IRepositoryBase<MemberProfileItem>
    {
        void UpdateProfile(int memberId, IList<MemberProfileItem> newItems);
        IQueryable<MemberProfileItem> GetByMemberId(int memberId);
        IQueryable<MemberProfileItem> GetByItemValueContains(int orgId, string itemValue);
    }

    public class MemberProfileItemRepository : RepositoryBase<MemberProfileItem, ApplicationDbContext>, IMemberProfileItemRepository
    {
        IRepositoryRead<Member> _memberReader;
        IRepositoryRead<MemberProfileItem> _memberProfileItemReader;

        public MemberProfileItemRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IRepositoryRead<Member> memberReader,
            IRepositoryRead<MemberProfileItem> memberProfileItemReader
        ) : base(unitOfWork)
        {
            _memberReader = memberReader;
            _memberProfileItemReader = memberProfileItemReader;
        }

        public class MemberProfileItemIdComparer : IEqualityComparer<MemberProfileItem>
        {
            public bool Equals(MemberProfileItem x, MemberProfileItem y)
            {
                if(x.Id == y.Id)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode(MemberProfileItem obj)
            {
                return obj.GetHashCode();
            }
        }

        public IQueryable<MemberProfileItem> GetByMemberId(int memberId)
        {
            return this.GetMany(i => i.MemberId == memberId);
        }

        /// <summary>
        /// * The <paramref name="memberId"/> must be an existing Member ID.
        /// * The "MemberId" of the items in the <paramref name="newItems"/> can be a non-positive value, which will be fixed automatically,
        ///   otherwise this value must equal to <paramref name="memberId" />
        /// </summary>
        public void UpdateProfile(int memberId, IList<MemberProfileItem> newItems)
        {
            var member = _memberReader.GetById(memberId);
            if(null == member)
            {
                throw new MemberNotExistsException(memberId);
            }

            foreach(var item in newItems)
            {
                if(item.MemberId <= 0)
                {
                    item.MemberId = memberId;
                }
                else if(item.MemberId != memberId)
                {
                    throw new MemberIdMismatchesException(memberId, item.MemberId);
                }
            }

            IList<MemberProfileItem> oldItems = member.MemberProfileItems;

            this.RemoveRange(oldItems.Except(newItems, new MemberProfileItemIdComparer()));
            this.UpdateRange(newItems.Intersect(oldItems, new MemberProfileItemIdComparer()));
            this.AddRange(newItems.Except(oldItems, new MemberProfileItemIdComparer()));
        }

        public IQueryable<MemberProfileItem> GetByItemValueContains(int orgId, string itemValue)
        {
            return from item in _memberProfileItemReader.GetAllNoTracking()
                   join member in _memberReader.GetManyNoTracking(m => m.CommunityId == orgId) on item.MemberId equals member.Id
                   where item.ItemValue.Contains(itemValue)
                   select item;
        }
    }
}
