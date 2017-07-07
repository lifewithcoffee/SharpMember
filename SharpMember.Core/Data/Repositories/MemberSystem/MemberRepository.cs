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
        Member GetMembersByMemberNumber(int orgId, int memberNumber);
        IQueryable<Member> GetMembers(int orgId, Expression<Func<Member, bool>> where);
        IQueryable<Member> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public Member GetMembersByMemberNumber(int orgId, int memberNumber)
        {
            return this.GetMany(m => m.MemberNumber == memberNumber && m.Organization.Id == orgId).SingleOrDefault();
        }

        public IQueryable<Member> GetMembers(int orgId, Expression<Func<Member, bool>> where)
        {
            return this.GetMany(m => m.Organization.Id == orgId).Where(where);
        }

        public IQueryable<Member> GetOrganizationMembersByItemValueMatching(int orgId, string itemValue)
        {
            return from item in this.UnitOfWork.Context.MemberProfileItems
                   where item.ItemValue.Contains(itemValue)
                   join member in this.GetMembers(orgId, m => true) on item.Member.Id equals member.Id
                   select member;
        }
    }
}
