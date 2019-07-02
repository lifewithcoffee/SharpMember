using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Definitions;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityCreateHandler
    {
        CommunityUpdateVm Get();
        Task<int> PostAsync(string appUserId, CommunityUpdateVm data);
    }

    public class CommunityCreateHandler : ICommunityCreateHandler
    {
        ICommunityService _communitySvc;
        IMemberService _memberSvc;
        IMemberProfileItemTemplateService _mpiTemplateSvc;

        public CommunityCreateHandler(
            ICommunityService communitySvc,
            IMemberService memberSvc,
            IMemberProfileItemTemplateService mpiTemplateSvc
        )
        {
            _communitySvc = communitySvc;
            _memberSvc = memberSvc;
            _mpiTemplateSvc = mpiTemplateSvc;
        }

        public CommunityUpdateVm Get()
        {
            CommunityUpdateVm model = new CommunityUpdateVm
            {
                ItemTemplateVMs = Enumerable.Range(0, 5).Select(i => new MemberProfileItemTemplateVm()).ToList()
            };

            return model;
        }

        public async Task<int> PostAsync(string appUserId, CommunityUpdateVm data)
        {
            Community community = new Community { Name = data.Name };
            _communitySvc.Repo.Add(community);
            await _communitySvc.CommitAsync();

            Member newMember = await _memberSvc.GenerateNewMemberWithProfileItemsAsync(community.Id, appUserId);
            newMember.CommunityRole = RoleNames.CommunityOwner;
            await _memberSvc.Repo.CommitAsync();

            var required = data.ItemTemplateVMs.Where(p => p.ItemTemplate.IsRequired == true).Select(p => p.ItemTemplate.ItemName);
            await _mpiTemplateSvc.AddTemplatesAsync(community.Id, required, true);

            var optional = data.ItemTemplateVMs.Where(p => p.ItemTemplate.IsRequired == false).Select(p => p.ItemTemplate.ItemName);
            await _mpiTemplateSvc.AddTemplatesAsync(community.Id, optional, false);

            await _mpiTemplateSvc.Repo.CommitAsync();
            return community.Id;
        }
    }
}
