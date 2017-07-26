using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;
using Microsoft.EntityFrameworkCore;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberRepository : IRepositoryBase<Member, ApplicationDbContext>
    {
        Member GenerateNewMember(int orgId);
        Member GetByMemberNumber(int orgId, int memberNumber);
        IQueryable<Member> GetByOrganization(int orgId);
        IQueryable<Member> GetByItemValue(int orgId, string itemValue);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public Member GenerateNewMember(int orgId)
        {
            if (null == this.UnitOfWork.Context.Organizations.Find(orgId))
            {
                throw new OrganizationNotExistsException(orgId);
            }

            int nextMemberNumber = this.GetMany(m => m.OrganizationId == orgId).OrderBy(m => m.MemberNumber).Last().MemberNumber + 1;

            var memberProfileItems = this.UnitOfWork.Context.MemberProfileItemTemplates
                .Where(t => t.OrganizationId == orgId)
                .Select(t => new MemberProfileItem { ItemName = t.ItemName })
                .ToList();

            Member returned = new Member
            {
                MemberNumber = nextMemberNumber,
                MemberProfileItems = memberProfileItems
            };

            return returned;
        }

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
                   join member in this.GetMany(m => m.OrganizationId == orgId) on item.MemberId equals member.Id
                   where item.ItemValue.Contains(itemValue)
                   select member;
        }
    }
}
