using NetCoreUtils.Database;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Definitions;
using System.Threading.Tasks;

namespace SharpMember.Core.Services
{
    public interface ICommunityService : ICommittable
    {
        Task AddMemberAsync(string appUserId, string name, string email, string role);
        Task AddMemberProfileTemplateAsync(string itemName, bool required);
        Task CreateCommunityAsync(string appUserId, string communityName);
        Community Community { get; set; }
    }

    class CommunityService :  ICommunityService
    {
        IUnitOfWork<ApplicationDbContext> _unitOfWork;
        ICommunityRepository _communityRepository;
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public Community Community { get; set; }

        public CommunityService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ICommunityRepository communityRepository,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        )
        {
            _unitOfWork = unitOfWork;
            _communityRepository = communityRepository;
            _memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task CreateCommunityAsync(string appUserId, string communityName)
        {
            this.Community = new Community { Name = communityName };
            this.Community.Members.Add(new Member { ApplicationUserId = appUserId, CommunityRole = RoleNames.CommunityOwner });
            _communityRepository.Add(this.Community);
            await _communityRepository.CommitAsync();
        }

        public async Task AddMemberAsync(string appUserId, string name, string email, string role)
        {
            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(this.Community.Id, appUserId);
            newMember.Name = name;
            newMember.Email = email;
            newMember.CommunityRole = role;
        }

        public async Task AddMemberProfileTemplateAsync(string itemName, bool required)
        {
            await _memberProfileItemTemplateRepository.AddTemplateAsync(this.Community.Id, itemName, required);
        }

        public bool Commit()
        {
            return _unitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _unitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}