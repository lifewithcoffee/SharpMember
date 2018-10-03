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
            var groupEditViewService = _fixture.GetService<IGroupEditViewService>();

            int memberVmNumber0 = groupEditViewService.Get(community.Groups[0].Id).ItemViewModels.Count;
            Assert.Equal(memberNumber0, memberVmNumber0);

            int memberVmNumber1 = groupEditViewService.Get(community.Groups[1].Id).ItemViewModels.Count;
            Assert.Equal(memberNumber1, memberVmNumber1);

            int memberVmNumber2 = groupEditViewService.Get(community.Groups[2].Id).ItemViewModels.Count;
            Assert.Equal(memberNumber2, memberVmNumber2);
        }
    }
}
