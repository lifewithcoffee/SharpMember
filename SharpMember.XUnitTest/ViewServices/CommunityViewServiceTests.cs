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
using System.Linq;

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
            Assert.Equal(5, model.ItemTemplateVMs.Count);    // initialize to have 5 empty item templates
        }

        [Fact]
        public async Task Community_create_view_post()
        {
            var (_, model_post) = await  _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            string newCommunityName = model_post.Name;
            string itemName1 = model_post.ItemTemplateVMs[0].ItemTemplate.ItemName;
            string itemName2 = model_post.ItemTemplateVMs[1].ItemTemplate.ItemName;
            int commId = model_post.Id;

            // verify
            var model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(commId, 0);
            Assert.Equal(commId, model_get.Id);
            Assert.Equal(newCommunityName, model_get.Name);

            Assert.Equal(2, model_get.ItemTemplateVMs.Count);

            Assert.Equal(commId, model_get.ItemTemplateVMs[0].ItemTemplate.CommunityId);
            Assert.True(model_get.ItemTemplateVMs[0].ItemTemplate.IsRequired);

            Assert.Equal(commId, model_get.ItemTemplateVMs[1].ItemTemplate.CommunityId);
            Assert.False(model_get.ItemTemplateVMs[1].ItemTemplate.IsRequired);

            Assert.True(model_get.ItemTemplateVMs[0].ItemTemplate.Id > 0);
            Assert.True(model_get.ItemTemplateVMs[1].ItemTemplate.Id > 0);

            Assert.Equal(itemName1, model_get.ItemTemplateVMs[0].ItemTemplate.ItemName);
            Assert.Equal(itemName2, model_get.ItemTemplateVMs[1].ItemTemplate.ItemName);

        }

        [Fact]
        public async Task Community_edit_view_service()
        { 
            var (_, model_post) = await  _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            string itemName1 = model_post.ItemTemplateVMs[0].ItemTemplate.ItemName;
            int commId = model_post.Id;

            // update item templates
            string updatedItem = ShortGuid.NewGuid();
            string appendedItem = ShortGuid.NewGuid();

            var editViewService_read = _fixture.GetServiceNewScope<ICommunityEditViewService>();
            var model_update = editViewService_read.Get(commId, 0);

            model_update.ItemTemplateVMs[1].ItemTemplate.ItemName = updatedItem;
            model_update.ItemTemplateVMs.Add(new MemberProfileItemTemplateVM { ItemTemplate = new MemberProfileItemTemplate { ItemName = appendedItem } });

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
            var model_get = editViewService.Get(commId, 0);

            Assert.Equal(3, model_get.ItemTemplateVMs.Count);

            Assert.Equal(itemName1, model_get.ItemTemplateVMs[0].ItemTemplate.ItemName);
            Assert.Equal(updatedItem, model_get.ItemTemplateVMs[1].ItemTemplate.ItemName);
            Assert.Equal(appendedItem, model_get.ItemTemplateVMs[2].ItemTemplate.ItemName);
        }

        [Fact]
        public async Task add_more_item_templates()
        {
            var (_, model_post) = await  _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            int itemTemplateNumber = model_post.ItemTemplateVMs.Where(x => !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Count();

            // when addMore = 0
            var model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_post.Id, 0);
            Assert.Equal(itemTemplateNumber, model_get.ItemTemplateVMs.Count);

            // when addMore = 10
            model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_post.Id, 10);
            Assert.Equal(itemTemplateNumber + 10, model_get.ItemTemplateVMs.Count);
        }

        [Fact]
        public async Task Existing_item_templates_with_empty_names_shouldNot_be_saved()
        {
            var (_, model_post) = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            // make change
            var model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_post.Id, 0);
            string currentName_before = model_get.ItemTemplateVMs[0].ItemTemplate.ItemName;

            model_get.ItemTemplateVMs[0].ItemTemplate.ItemName = "   ";
            await _fixture.GetServiceNewScope<ICommunityEditViewService>().PostAsync(model_get);

            // verify
            var model_get_after = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_post.Id, 0);
            string currentName_after = model_get_after.ItemTemplateVMs[0].ItemTemplate.ItemName;

            Assert.Equal(currentName_before, currentName_after);
        }

        [Fact]
        public async Task New_item_templates_with_empty_names_shouldNot_be_saved()
        { 
            var (_, model_post) = await _fixture.GetService<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            int vmSaved = model_post.ItemTemplateVMs.Where(x => !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Count();

            // add more item templates with blank names
            var model_get = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_post.Id, 7);
            int vmCount = model_get.ItemTemplateVMs.Count;
            model_get.ItemTemplateVMs[vmCount - 1].ItemTemplate.ItemName = "  ";
            model_get.ItemTemplateVMs[vmCount - 2].ItemTemplate.ItemName = "";
            model_get.ItemTemplateVMs[vmCount - 3].ItemTemplate.ItemName = ShortGuid.NewGuid(); // only this should be saved
            await _fixture.GetServiceNewScope<ICommunityEditViewService>().PostAsync(model_get);

            // verify
            var model_get2 = _fixture.GetServiceNewScope<ICommunityEditViewService>().Get(model_get.Id, 0);
            int vmCount2 = model_get2.ItemTemplateVMs.Count;
            Assert.Equal(vmSaved + 1, vmCount2);
        }
    }
}
