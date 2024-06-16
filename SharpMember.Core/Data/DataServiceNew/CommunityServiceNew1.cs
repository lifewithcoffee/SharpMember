using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DataServiceNew
{
    public class CommunityServiceNew1 : ICommunityServiceNew
    {
        int _communityId;

        IRepository<Member> _memberRepo;
        IRepository<Community> _communityRepo;
        IRepositoryRead<MemberProfileItemTemplate> _memberProfileItemTemplateReader;

        public ICommunityServiceNew Bind(int id)
        {
            if (!_communityRepo.Exist(e => e.Id == id))
            {
                throw new CommunityNotExistsException(id);
            }

            _communityId = id;
            return this;
        }

        public async Task<Member> AddMemberAsync(string appUserId, string name, string email, string role)
        {
            Member newMember = await this.CreateMemberAsync(appUserId);
            newMember.Name = name;
            newMember.Email = email;
            newMember.CommunityRole = role;
            _memberRepo.Add(newMember);
            await _memberRepo.CommitAsync();
            return newMember;
        }

        public Task AddMemberProfileTemplateAsync(string itemName, bool required)
        {
            throw new NotImplementedException(); // TODO: AddMemberProfileTemplateAsync
        }

        public async Task CreateCommunityAsync(string appUserId, string communityName)
        {
            var community = new Community { Name = communityName };
            community.Members.Add(new Member { ApplicationUserId = appUserId, CommunityRole = RoleNames.CommunityOwner });
            _communityRepo.Add(community);
            await _communityRepo.CommitAsync();
        }

        public async Task<Member> CreateMemberAsync(string appUserId)
        {
            var memberProfileItems = await _memberProfileItemTemplateReader
                                            .Query(t => t.CommunityId == _communityId)
                                            .AsNoTracking()
                                            .Select(t => new MemberProfileItem { MemberProfileItemTemplateId = t.Id })
                                            .ToListAsync();

            Member returned = new Member
            {
                MemberProfileItems = memberProfileItems,
                CommunityId = _communityId,
                ApplicationUserId = appUserId
            };

            return returned;
        }

        public IQueryable<Member> QueryMembers()
        {
            return _memberRepo.Query(m => m.CommunityId == _communityId);
        }

        public IQueryable<Member> QueryMemberByNumber(int memberNumber)
        {
            return _memberRepo.Query(m => m.MemberNumber == memberNumber && m.CommunityId == _communityId);
        }

        public int GetNextUnassignedMemberNumber()
        {
            int nextMemberNumber = 1;
            var member = _memberRepo.Query(m => m.CommunityId == _communityId).OrderBy(m => m.MemberNumber).LastOrDefault();
            if (member != null)
            {
                nextMemberNumber = member.MemberNumber + 1;
            }
            return nextMemberNumber;
        }
    }
}
