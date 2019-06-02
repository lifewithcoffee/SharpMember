using NetCoreUtils.Database;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Definitions;
using System;
using System.Threading.Tasks;

namespace SharpMember.Core.Services
{
    public interface ICommunityService : ICommittable
    {
        Task<Member> AddMemberAsync(string appUserId, string name, string email, string role);
        Task AddMemberProfileTemplateAsync(string itemName, bool required);
        Task CreateCommunityAsync(string appUserId, string communityName);
        Community Community { get; set; }
    }

    public class EntityServiceBase : ICommittable
    {
        IUnitOfWork<ApplicationDbContext> _unitOfWork;
        public EntityServiceBase(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Commit()
        {
            return _unitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }

    class CommunityService : EntityServiceBase, ICommunityService
    {
        IRepositoryBase<Community> _communityRepo;
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public Community Community { get; set; }

        public CommunityService(
            IRepositoryBase<Community> communityRepo,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ):base(unitOfWork)
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