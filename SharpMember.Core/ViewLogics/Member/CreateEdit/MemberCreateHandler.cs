using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Member;
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
        readonly IMemberService _memberRepository;
        readonly IMemberProfileItemTemplateService _mpiTemplateSvc;

        public MemberCreateHandler(
            IMemberService memberRepo,
            IMemberProfileItemTemplateService mpiTemplateSvc
        )
        {
            _memberRepository = memberRepo;
            _mpiTemplateSvc = mpiTemplateSvc;
        }

        public async Task<MemberUpdateVm> GetAsync(int commId, string appUserId)
        {
            var member = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(commId, appUserId);
            var result = new MemberUpdateVm().CopyFrom(member);
            result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _mpiTemplateSvc.Repo);
            return result;
        }

        public async Task<int> Post(MemberUpdateVm data)
        {
            Member member = new Member().CopyFrom(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _mpiTemplateSvc.Repo);

            _memberRepository.Add(member);
            await _mpiTemplateSvc.CommitAsync();

            return member.Id;
        }
    }
}
