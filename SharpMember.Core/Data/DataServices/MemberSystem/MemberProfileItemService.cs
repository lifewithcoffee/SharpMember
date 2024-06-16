using SharpMember.Core.Data.Models;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Definitions;
using Microsoft.EntityFrameworkCore;

namespace SharpMember.Core.Data.DataServices.MemberSystem
{
    public interface IMemberProfileItemService : ICommittable
    {
        void UpdateProfile(int memberId, IList<MemberProfileItem> newItems);
        IQueryable<MemberProfileItem> GetByMemberId(int memberId);
        IQueryable<MemberProfileItem> GetByItemValueContains(int orgId, string itemValue);
    }

    public class MemberProfileItemService : EntityServiceBase<MemberProfileItem>, IMemberProfileItemService
    {
        readonly IRepositoryRead<Member> _memberReader;

        public MemberProfileItemService(
            IRepository<MemberProfileItem> memberProfileItemRepo,
            IRepositoryRead<Member> memberReader
        ):base(memberProfileItemRepo)
        {
            _memberReader = memberReader;
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
            return _repo.Query(i => i.MemberId == memberId);
        }

        /// <summary>
        /// * The <paramref name="memberId"/> must be an existing Member ID.
        /// * The "MemberId" of the items in the <paramref name="newItems"/> can be a non-positive value, which will be fixed automatically,
        ///   otherwise this value must equal to <paramref name="memberId" />
        /// </summary>
        public void UpdateProfile(int memberId, IList<MemberProfileItem> newItems)
        {
            var member = _memberReader.Get(memberId);
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

            _repo.RemoveRange(oldItems.Except(newItems, new MemberProfileItemIdComparer()));
            _repo.UpdateRange(newItems.Intersect(oldItems, new MemberProfileItemIdComparer()));
            _repo.AddRange(newItems.Except(oldItems, new MemberProfileItemIdComparer()));
        }

        public IQueryable<MemberProfileItem> GetByItemValueContains(int orgId, string itemValue)
        {
            return from item in _repo.QueryAll().AsNoTracking()
                   join member in _memberReader.Query(m => m.CommunityId == orgId).AsNoTracking() on item.MemberId equals member.Id
                   where item.ItemValue.Contains(itemValue)
                   select item;
        }
    }
}
