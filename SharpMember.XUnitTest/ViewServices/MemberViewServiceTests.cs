using SharpMember.Core.Views.ViewServices.MemberViewServices;
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
    public class MemberViewServiceTests
    {
        ServiceProviderFixture _fixture;

        public MemberViewServiceTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Member_profile_should_be_consist_with_member_profile_template()
        {
            var (appUserId, model_post) = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            // case 1: create by user with appUserId
            var model1 = await _fixture.GetService<IMemberCreateViewService>().GetAsync(model_post.Id, appUserId);
            Assert.Equal(2, model1.ProfileItemViewModels.Count);

            // case 2: create by admin with no appUserId
            var model2 = await _fixture.GetService<IMemberCreateViewService>().GetAsync(model_post.Id, null);
            Assert.Equal(2, model2.ProfileItemViewModels.Count);
        }
    }
}