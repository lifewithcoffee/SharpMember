using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemRepository : IRepositoryBase<MemberProfileItem, ApplicationDbContext> { }

    public class MemberProfileItemRepository : RepositoryBase<MemberProfileItem, ApplicationDbContext>, IMemberProfileItemRepository
    {
        public MemberProfileItemRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public class IdComparer : IEqualityComparer<MemberProfileItem>
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

        public void UpdateProfile(int memberId, IList<MemberProfileItem> oldItems, IList<MemberProfileItem> newItems)
        {
            this.DeleteRange(oldItems.Except(newItems, new IdComparer()));
            this.UpdateRange(newItems.Intersect(oldItems, new IdComparer()));
            this.AddRange(newItems.Except(oldItems, new IdComparer()));
        }
    }
}
