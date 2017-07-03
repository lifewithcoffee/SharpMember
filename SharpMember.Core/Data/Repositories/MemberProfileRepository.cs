using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberManagement;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberProfileRepository : IRepositoryBase<MemberProfile, ApplicationDbContext>
    {
        MemberProfile GetOrganizationMemberByMemberNumber(int orgId, int memberNumber);
        IQueryable<MemberProfile> GetOrganizationMembers(int orgId, Expression<Func<MemberProfile, bool>> where);
        IQueryable<MemberProfile> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue);
    }

    public class MemberProfileRepository : RepositoryBase<MemberProfile, ApplicationDbContext>, IMemberProfileRepository
    {
        public MemberProfileRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public MemberProfile GetOrganizationMemberByMemberNumber(int orgId, int memberNumber)
        {
            return this.GetMany(m => m.MemberNumber == memberNumber && m.Branch.Organization.Id == orgId).SingleOrDefault();
        }

        public IQueryable<MemberProfile> GetOrganizationMembers(int orgId, Expression<Func<MemberProfile, bool>> where)
        {
            return this.UnitOfWork.Context.Branches.Where(b => b.Organization.Id == orgId).SelectMany(b => b.Members).Where(where);
        }

        public IQueryable<MemberProfile> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue)
        {
            return from item in this.UnitOfWork.Context.MemberProfileItems
                   where item.ItemValue.Contains(itemValue)
                   join member in this.GetOrganizationMembers(orgId, m => true) on item.MemberProfile.Id equals member.Id
                   select member;
        }
    }
}
