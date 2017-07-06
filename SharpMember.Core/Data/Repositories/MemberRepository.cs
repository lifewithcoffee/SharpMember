using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberRepository : IRepositoryBase<Member, ApplicationDbContext>
    {
        Member GetOrganizationMemberByMemberNumber(int orgId, int memberNumber);
        IQueryable<Member> GetOrganizationMembers(int orgId, Expression<Func<Member, bool>> where);
        IQueryable<Member> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public Member GetOrganizationMemberByMemberNumber(int orgId, int memberNumber)
        {
            return this.GetMany(m => m.MemberNumber == memberNumber && m.Branch.Organization.Id == orgId).SingleOrDefault();
        }

        public IQueryable<Member> GetOrganizationMembers(int orgId, Expression<Func<Member, bool>> where)
        {
            return this.UnitOfWork.Context.Branches.Where(b => b.Organization.Id == orgId).SelectMany(b => b.Members).Where(where);
        }

        public IQueryable<Member> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue)
        {
            return from item in this.UnitOfWork.Context.MemberProfileItems
                   where item.ItemValue.Contains(itemValue)
                   join member in this.GetOrganizationMembers(orgId, m => true) on item.Member.Id equals member.Id
                   select member;
        }
    }
}
