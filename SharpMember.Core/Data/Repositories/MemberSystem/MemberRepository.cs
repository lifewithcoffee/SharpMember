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
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberRepository : IRepositoryBase<Member, ApplicationDbContext>
    {
        Member GenerateNewMember(int orgId);
        Member GetByMemberNumber(int orgId, int memberNumber);
        Task<int> AssignMemberNubmer(int memberId, int nextMemberNumber);
        IQueryable<Member> GetByOrganization(int orgId);
        IQueryable<Member> GetByItemValue(int orgId, string itemValue);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        /// <summary>
        /// Try to assign the <paramref name="nextMemberNumber"/> to the relevant member.
        /// 
        ///     * If <paramref name="nextMemberNumber"/> is zero or an negative value, a new
        ///       <paramref name="nextMemberNumber"/> will be queried from the database.
        ///       
        ///     * After saving the change to the database, the method will check if there is
        ///       a duplication and try to resolve it if a duplication is found.
        ///       
        /// </summary>
        /// <returns>The successfully assigned member number.</returns>
        public async Task<int> AssignMemberNubmer(int memberId, int nextMemberNumber)
        {
            var member = this.GetById(memberId);
            
            if(nextMemberNumber <= 0)
            {
                nextMemberNumber = this.GetMany(m => m.OrganizationId == member.OrganizationId).OrderBy(m => m.MemberNumber).Last().MemberNumber + 1;
            }
            
            if(member.MemberNumber <= 0)
            {
                member.MemberNumber = nextMemberNumber;
            }

            await this.CommitAsync();

            // check if there is a duplication
            while(this.GetMany(m => m.MemberNumber == nextMemberNumber).Count() > 1)
            {
                nextMemberNumber = await AssignMemberNubmer(memberId, 0);
            }

            return nextMemberNumber;
        }

        public Member GenerateNewMember(int orgId)
        {
            if (null == this.UnitOfWork.Context.Organizations.Find(orgId))
            {
                throw new OrganizationNotExistsException(orgId);
            }

            var memberProfileItems = this.UnitOfWork.Context.MemberProfileItemTemplates
                .Where(t => t.OrganizationId == orgId)
                .Select(t => new MemberProfileItem { ItemName = t.ItemName })
                .ToList();

            Member returned = new Member { MemberProfileItems = memberProfileItems };

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
