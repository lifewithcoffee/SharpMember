using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberRepository : IRepositoryBase<Member, ApplicationDbContext>
    {
        Member GetByMemberNumber(int orgId, int memberNumber);
        IQueryable<Member> GetByOrganization(int orgId);
        IQueryable<Member> GetByItemValue(int orgId, string itemValue);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public Member GetByMemberNumber(int orgId, int memberNumber)
        {
            return this.GetMany(m => m.MemberNumber == memberNumber && m.OrganizationId == orgId).SingleOrDefault();
        }

        public IQueryable<Member> GetByOrganization(int orgId)
        {
            return this.GetMany(m => m.OrganizationId == orgId);
        }

        public IQueryable<Member> GetByItemValue(int orgId, string itemValue)
        {
            return from item in this.UnitOfWork.Context.MemberProfileItems
                   join memberProfile in this.UnitOfWork.Context.MemberProfiles on item.MemberProfileId equals memberProfile.Id
                   join member in this.GetMany(m => m.OrganizationId == orgId) on memberProfile.Id equals member.MemberProfileId
                   where item.ItemValue.Contains(itemValue)
                   select member;
        }
    }
}
