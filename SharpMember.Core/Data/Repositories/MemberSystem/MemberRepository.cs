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
        int GetNextUnassignedMemberNumber(int orgId);
        IQueryable<Member> GetByMemberNumber(int orgId, int memberNumber);
        IQueryable<Member> GetByOrganization(int orgId);
        Task<Member> GenerateNewMemberWithProfileItemsAsync(int orgId);
        Task<int> AssignMemberNubmerAsync(int memberId, int nextMemberNumber);
    }

    public class MemberRepository : RepositoryBase<Member, ApplicationDbContext>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public override Member Add(Member entity)
        {
            if (null == this.UnitOfWork.Context.Organizations.Find(entity.OrganizationId))
            {
                throw new OrganizationNotExistsException(entity.OrganizationId);
            }
            return base.Add(entity);
        }

        public int GetNextUnassignedMemberNumber(int orgId)
        {
            int nextMemberNumber = 1;
            var member = this.GetMany(m => m.OrganizationId == orgId).OrderBy(m => m.MemberNumber).LastOrDefault();
            if(member != null)
            {
                nextMemberNumber = member.MemberNumber + 1;
            }
            return nextMemberNumber;
        }

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
        public async Task<int> AssignMemberNubmerAsync(int memberId, int nextMemberNumber)
        {
            var member = this.GetById(memberId);
            
            if(nextMemberNumber <= 0)
            {
                nextMemberNumber = this.GetNextUnassignedMemberNumber(member.OrganizationId);
            }
            
            if(member.MemberNumber <= 0)
            {
                member.MemberNumber = nextMemberNumber;
            }

            await this.CommitAsync();

            // check if there is a duplication
            while(this.GetByMemberNumber(member.OrganizationId, nextMemberNumber).Count() > 1)
            {
                nextMemberNumber = await AssignMemberNubmerAsync(memberId, 0);
                member.MemberNumber = nextMemberNumber;
                await this.CommitAsync();
            }

            return nextMemberNumber;
        }

        public async Task<Member> GenerateNewMemberWithProfileItemsAsync(int orgId)
        {
            if (null == this.UnitOfWork.Context.Organizations.Find(orgId))
            {
                throw new OrganizationNotExistsException(orgId);
            }

            var memberProfileItems = await this.UnitOfWork.Context.MemberProfileItemTemplates
                .Where(t => t.OrganizationId == orgId)
                .Select(t => new MemberProfileItem { ItemName = t.ItemName })
                .ToListAsync();

            Member returned = new Member { MemberProfileItems = memberProfileItems, OrganizationId = orgId};

            return returned;
        }

        public IQueryable<Member> GetByMemberNumber(int orgId, int memberNumber)
        {
            return this.GetMany(m => m.MemberNumber == memberNumber && m.OrganizationId == orgId);
        }

        public IQueryable<Member> GetByOrganization(int orgId)
        {
            return this.GetMany(m => m.OrganizationId == orgId);
        }

       
    }
}
