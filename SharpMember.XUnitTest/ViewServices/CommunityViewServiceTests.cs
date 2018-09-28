using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Views.ViewModels;
using System.Threading.Tasks;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using U.TestEnv;
using U.TestEnv.TestService;
using NetCoreUtils.String;

namespace U.ViewServices
{
    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityViewServiceTests
    {
        ServiceProviderFixture _fixture;

        public CommunityViewServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
            this._fixture = serviceProviderFixture;
        }

        [Fact]
        public void Community_create_view_get()
        {
            var createViewService = _fixture.GetServiceNewScope<ICommunityCreateViewService>();
            CommunityUpdateVM model = createViewService.Get();
            Assert.Equal(0, model.Id);
            Assert.True(string.IsNullOrWhiteSpace(model.Name));
            Assert.Equal(5, model.MemberProfileItemTemplates.Count);    // initialize to have 5 empty item templates
        }

        [Fact]
        public async Task Community_create_view_post()
        {
            var (_, model_post) = await  _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            string newCommunityName = model_post.Name;
            string itemName1 = model_post.MemberProfileItemTemplates[0].ItemName;
            string itemName2 = model_post.MemberProfileItemTemplates[1].ItemName;
            int commId = model_post.Id;

            // verify
            var model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(commId);
            Assert.Equal(commId, model_get.Id);
            Assert.Equal(newCommunityName, model_get.Name);

            Assert.Equal(2, model_get.MemberProfileItemTemplates.Count);

            Assert.Equal(commId, model_get.MemberProfileItemTemplates[0].CommunityId);
            Assert.True(model_get.MemberProfileItemTemplates[0].IsRequired);

            Assert.Equal(commId, model_get.MemberProfileItemTemplates[1].CommunityId);
            Assert.False(model_get.MemberProfileItemTemplates[1].IsRequired);

            Assert.True(model_get.MemberProfileItemTemplates[0].Id > 0);
            Assert.True(model_get.MemberProfileItemTemplates[1].Id > 0);

            Assert.Equal(itemName1, model_get.MemberProfileItemTemplates[0].ItemName);
            Assert.Equal(itemName2, model_get.MemberProfileItemTemplates[1].ItemName);

        }

        [Fact]
        public async Task Community_edit_view_service()
        { 
            var (_, model_post) = await  _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            string itemName1 = model_post.MemberProfileItemTemplates[0].ItemName;
            int commId = model_post.Id;

            // update item templates
            string updatedItem = ShortGuid.NewGuid();
            string appendedItem = ShortGuid.NewGuid();

            var editViewService_read = _fixture.GetServiceNewScope<ICommunityEditViewService>();
            var model_update = editViewService_read.Get(commId);

            model_update.MemberProfileItemTemplates[1].ItemName = updatedItem;
            model_update.MemberProfileItemTemplates.Add(new MemberProfileItemTemplate { ItemName = appendedItem });

            /** write changes
             * 
             * Note:
             * 
             *  if reuse the 'editViewService_read' above to do PostAsync(), the following exception
             *  will be thrown:
             *  
             *      The instance of entity type … cannot be tracked because another instance of this
             *      type with the same key is already being tracked
             *      
             *  The reason is that the change is not applied directly to the retrieved data entity but 
             *  a converted view model object. Then the view model object is converted back to a new
             *  data entity, which has the same ID with the one retrieved before.
             *  
             *  However, when the new data entity is posted, the previous one is still tracked by the
             *  same DbContext. That's why the above exception is thrown.
             *  
             *  The workaround is simple -- post using a different DbContext. That's why the creating
             *  a new scope to get a ICommunityEditViewService instance can fix this problem.
             */
            var editViewService_write = _fixture.GetServiceNewScope<ICommunityEditViewService>();
            await editViewService_write.PostAsync(model_update);

            // verify
            var editViewService = _fixture.GetServiceNewScope<ICommunityEditViewService>();
            var model_get = editViewService.Get(commId);

            Assert.Equal(3, model_get.MemberProfileItemTemplates.Count);

            Assert.Equal(itemName1, model_get.MemberProfileItemTemplates[0].ItemName);
            Assert.Equal(updatedItem, model_get.MemberProfileItemTemplates[1].ItemName);
            Assert.Equal(appendedItem, model_get.MemberProfileItemTemplates[2].ItemName);
        }
    }
}
