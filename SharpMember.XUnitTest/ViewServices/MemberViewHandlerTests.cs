using SharpMember.Core.Views.ViewServices.CommunityViewServices;
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
    public class MemberViewHandlerTests
    {
        readonly ServiceProviderFixture _fixture;

        public MemberViewHandlerTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Member_profileItemNumber_should_beTheSameWith_profileTemplateNumber_when_creating()
        {
            var (appUserId, model_post) = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            // case 1: create by user with appUserId
            var model1 = await _fixture.GetServiceNewScope<IMemberCreateHandler>().GetAsync(model_post.Id, appUserId);
            Assert.Equal(2, model1.ProfileItemViewModels.Count);

            // case 2: create by admin with no appUserId
            var model2 = await _fixture.GetServiceNewScope<IMemberCreateHandler>().GetAsync(model_post.Id, null);
            Assert.Equal(2, model2.ProfileItemViewModels.Count);
        }

        [Fact]
        public async Task Delete_profileItemTemplate_should_alsoDelete_profileItems_from_allMembers()
        {
            var (_, model_post) = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            var model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 0);
            int templateNumberBeforeDelete = model_get.ItemTemplateVMs.Count;
            model_get.ItemTemplateVMs[0].Delete = true;
            model_get.ItemTemplateVMs[1].Delete = true;
            await _fixture.GetServiceNewScope<ICommunityEditHandler>().PostAsync(model_get);

            int templateNumberAfterDelete = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 0).ItemTemplateVMs.Count;
            Assert.Equal(templateNumberAfterDelete, templateNumberBeforeDelete - 2);
        }
    }
}