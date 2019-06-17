using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Utils.Mappers;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.MemberViewServices
{
    public interface IMemberCreateHandler
    {
        Task<MemberUpdateVm> GetAsync(int commId, string appUserId);
        Task<int> Post(MemberUpdateVm data);
    }

    public class MemberCreateHandler : IMemberCreateHandler
    {
        IMemberRepository _memberRepository;
        IRepository<MemberProfileItemTemplate> _memberProfileItemTemplateRepository;

        public MemberCreateHandler(
            IMemberRepository memberRepo,
            IRepository<MemberProfileItemTemplate> memberProfileItemTemplateRepository
        )
        {
            _memberRepository = memberRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task<MemberUpdateVm> GetAsync(int commId, string appUserId)
        {
            var member = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(commId, appUserId);
            var result = new MemberUpdateVm().CopyFrom(member);
            result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _memberProfileItemTemplateRepository);
            return result;
        }

        public async Task<int> Post(MemberUpdateVm data)
        {
            Member member = new Member().CopyFrom(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _memberProfileItemTemplateRepository);

            _memberRepository.Add(member);
            await _memberProfileItemTemplateRepository.CommitAsync();

            return member.Id;
        }
    }
}
