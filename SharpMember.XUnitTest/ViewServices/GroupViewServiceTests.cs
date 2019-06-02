using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewServices.GroupViewServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using U.TestEnv;
using U.TestEnv.TestService;
using Xunit;

namespace U.ViewServices
{
    [Collection(nameof(ServiceProviderCollection))]
    public class GroupViewServiceTests
    {
        ServiceProviderFixture _fixture;

        public GroupViewServiceTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Group_editView_should_contain_members()
        {
            // create test community with 3 groups
            var community = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromRepository();

            int memberNumber0 = community.Groups[0].GroupMemberRelations.Count;
            Assert.True(memberNumber0 > 0);

            int memberNumber1 = community.Groups[1].GroupMemberRelations.Count;
            Assert.True(memberNumber1 > 0);

            int memberNumber2 = community.Groups[2].GroupMemberRelations.Count;
            Assert.True(memberNumber2 > 0);

            // verify
            var groupEditViewService = _fixture.GetService<IGroupEditHandler>();

            int memberVmNumber0 = groupEditViewService.Get(community.Groups[0].Id).MemberItemVms.Count;
            Assert.Equal(memberNumber0, memberVmNumber0);

            int memberVmNumber1 = groupEditViewService.Get(community.Groups[1].Id).MemberItemVms.Count;
            Assert.Equal(memberNumber1, memberVmNumber1);

            int memberVmNumber2 = groupEditViewService.Get(community.Groups[2].Id).MemberItemVms.Count;
            Assert.Equal(memberNumber2, memberVmNumber2);
        }

        [Fact]
        public async Task Group_editView_delete_selected_members()
        {
            // populate testing data
            var community = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromRepository();
            int totalMemberNumberBefore = community.Members.Count;
            int groupMemberNumberBefore = community.Groups[2].GroupMemberRelations.Count;
            Assert.True(totalMemberNumberBefore > 0);
            Assert.True(groupMemberNumberBefore > 0);

            // do change: remove members from group
            var viewModel = _fixture.GetService<IGroupEditHandler>().Get(community.Groups[2].Id);
            viewModel.MemberItemVms[0].Selected = true;
            viewModel.MemberItemVms[1].Selected = true;
            await _fixture.GetServiceNewScope<IGroupEditHandler>().PostToDeleteSelectedMembersAsync(viewModel);

            // verify: members are removed from group
            var viewModel2 = _fixture.GetServiceNewScope<IGroupEditHandler>().Get(community.Groups[2].Id);
            int groupMemberNumberAfter = viewModel2.MemberItemVms.Count;
            Assert.Equal(groupMemberNumberBefore - 2, groupMemberNumberAfter);

            // verify: make sure the members are not entirely deleted from database
            int totalMemberNumberAfter = await _fixture.GetServiceNewScope<IMemberRepository>().Repo.GetMany(x => x.CommunityId == community.Id).CountAsync();
            Assert.Equal(totalMemberNumberBefore, totalMemberNumberAfter);
        }

        [Fact]
        public async Task Group_addMemberView_should_only_list_members_not_in_the_current_group()
        {
            // populate testing data
            var community = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromRepository();
            int totalMemberNumber = community.Members.Count;
            int groupMemberNumber = community.Groups[2].GroupMemberRelations.Count;

            // get addMember view's item number
            int groupId = community.Groups[2].Id;
            var vm = _fixture.GetServiceNewScope<IGroupAddMemberHandler>().Get(groupId);
            int viewItemNumber = vm.MemberItemVms.Count;

            // verify
            Assert.Equal(totalMemberNumber - groupMemberNumber, viewItemNumber);
        }
    }
}
