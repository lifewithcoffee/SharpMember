using NetCoreUtils.Database;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Definitions;
using System;
using System.Threading.Tasks;
using SharpMember.Core.Data.DataServices;

namespace SharpMember.Core.Data.DataServices.MemberSystem
{
    public interface ICommunityService : ICommittable
    {
        Task<Member> AddMemberAsync(string appUserId, string name, string email, string role);
        Task AddMemberProfileTemplateAsync(string itemName, bool required);
        Task CreateCommunityAsync(string appUserId, string communityName);
        Community Community { get; set; }
        IRepository<Community> Repo { get; }
    }

   

    class CommunityService : EntityServiceBase<Community>, ICommunityService
    {
        readonly IRepository<Community> _communityRepo;
        readonly IMemberRepository _memberRepository;
        readonly IMemberProfileItemTemplateService _memberProfileItemTemplateRepository;

        public Community Community { get; set; }

        public CommunityService(
            IRepository<Community> communityRepo,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateService memberProfileItemTemplateRepository
        ):base( communityRepo)
        {
            _communityRepo = communityRepo;
            _memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task CreateCommunityAsync(string appUserId, string communityName)
        {
            this.Community = new Community { Name = communityName };
            this.Community.Members.Add(new Member { ApplicationUserId = appUserId, CommunityRole = RoleNames.CommunityOwner });
            _communityRepo.Add(this.Community);
            await _communityRepo.CommitAsync();
        }

        public async Task<Member> AddMemberAsync(string appUserId, string name, string email, string role)
        {
            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(this.Community.Id, appUserId);
            newMember.Name = name;
            newMember.Email = email;
            newMember.CommunityRole = role;
            _memberRepository.Add(newMember);
            return newMember;
        }

        public async Task AddMemberProfileTemplateAsync(string itemName, bool required)
        {
            await _memberProfileItemTemplateRepository.AddTemplateAsync(this.Community.Id, itemName, required);
        }

    }
}