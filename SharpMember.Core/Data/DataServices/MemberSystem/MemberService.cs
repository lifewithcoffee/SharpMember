using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.Member;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SharpMember.Core.Definitions;

namespace SharpMember.Core.Data.DataServices.MemberSystem
{
    public interface IMemberService : ICommittable
    {
        int GetNextUnassignedMemberNumber(int commId);
        IQueryable<Member> GetByMemberNumber(int commId, int memberNumber);
        IQueryable<Member> GetByCommunity(int commId);
        Task<Member> GenerateNewMemberWithProfileItemsAsync(int commId, string appUserId);
        Task<int> AssignMemberNubmerAsync(int memberId, int nextMemberNumber);
        Member Add(Member entity);
        IRepository<Member> Repo { get; }
    }

    public class MemberService : EntityServiceBase<Member>, IMemberService
    {
        private readonly IRepositoryRead<Community> _communityReader;
        private readonly IRepositoryRead<MemberProfileItemTemplate> _memberProfileItemTemplateReader;

        public MemberService(
            IRepository<Member> repo,
            IRepositoryRead<Community> communityRepoReader,
            IRepositoryRead<MemberProfileItemTemplate> memberProfileItemTemplateRepoReader
        ):base(repo)
        {
            _communityReader = communityRepoReader;
            _memberProfileItemTemplateReader = memberProfileItemTemplateRepoReader;
        }

        public Member Add(Member entity)
        {
            if(!_communityReader.Exist(e => e.Id == entity.CommunityId))
            {
                throw new CommunityNotExistsException(entity.CommunityId);
            }
            return _repo.Add(entity);
        }

        public int GetNextUnassignedMemberNumber(int commId)
        {
            int nextMemberNumber = 1;
            var member = _repo.Query(m => m.CommunityId == commId).OrderBy(m => m.MemberNumber).LastOrDefault();
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
            var member = _repo.Get(memberId);
            
            if(nextMemberNumber <= 0)
            {
                nextMemberNumber = this.GetNextUnassignedMemberNumber(member.CommunityId);
            }
            
            if(member.MemberNumber <= 0)
            {
                member.MemberNumber = nextMemberNumber;
            }

            await _repo.CommitAsync();

            // check if there is a duplication
            while(this.GetByMemberNumber(member.CommunityId, nextMemberNumber).Count() > 1)
            {
                nextMemberNumber = await AssignMemberNubmerAsync(memberId, 0);
                member.MemberNumber = nextMemberNumber;
                await _repo.CommitAsync();
            }

            return nextMemberNumber;
        }

        public async Task<Member> GenerateNewMemberWithProfileItemsAsync(int commId, string appUserId)
        {
            if(!_communityReader.Exist(e => e.Id == commId))
            {
                throw new CommunityNotExistsException(commId);
            }

            var memberProfileItems = await _memberProfileItemTemplateReader.Query(t => t.CommunityId == commId)
                                                                           .AsNoTracking()
                                                                           .Select(t => new MemberProfileItem { MemberProfileItemTemplateId = t.Id })
                                                                           .ToListAsync();
            Member returned = new Member { MemberProfileItems = memberProfileItems, CommunityId = commId, ApplicationUserId = appUserId};

            return returned;
        }

        public IQueryable<Member> GetByMemberNumber(int orgId, int memberNumber)
        {
            return _repo.Query(m => m.MemberNumber == memberNumber && m.CommunityId == orgId);
        }

        public IQueryable<Member> GetByCommunity(int commId)
        {
            return _repo.Query(m => m.CommunityId == commId);
        }
    }
}
