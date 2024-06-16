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
using SharpMember.Core.Views.ViewModels.CommunityVms;
using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Community;

namespace U.ViewHandlers
{
    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityCreateHandler_Tests
    {
        readonly ServiceProviderFixture _fixture;

        public CommunityCreateHandler_Tests(ServiceProviderFixture serviceProviderFixture)
        {
            this._fixture = serviceProviderFixture;
        }

        [Fact]
        public void Community_create_view_get()
        {
            var createViewService = _fixture.GetServiceNewScope<ICommunityCreateHandler>();
            CommunityUpdateVm model = createViewService.Get();
            Assert.Equal(0, model.Id);
            Assert.True(string.IsNullOrWhiteSpace(model.Name));
            Assert.Equal(5, model.ItemTemplateVMs.Count);    // initialize to have 5 empty item templates
        }

        [Fact]
        public async Task Community_create_view_post()
        {
            var (_, model_post) = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            string newCommunityName = model_post.Name;

            string itemName0 = model_post.ItemTemplateVMs[0].ItemTemplate.ItemName;
            bool required0 = model_post.ItemTemplateVMs[0].ItemTemplate.IsRequired;

            string itemName1 = model_post.ItemTemplateVMs[1].ItemTemplate.ItemName;
            bool required1 = model_post.ItemTemplateVMs[1].ItemTemplate.IsRequired;

            int commId = model_post.Id;

            // verify
            var model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(commId, 0);
            Assert.Equal(commId, model_get.Id);
            Assert.Equal(newCommunityName, model_get.Name);

            Assert.Equal(2, model_get.ItemTemplateVMs.Count);

            Assert.Equal(commId, model_get.ItemTemplateVMs[0].ItemTemplate.CommunityId);
            Assert.Equal(commId, model_get.ItemTemplateVMs[1].ItemTemplate.CommunityId);

            Assert.True(model_get.ItemTemplateVMs[0].ItemTemplate.Id > 0);
            Assert.True(model_get.ItemTemplateVMs[1].ItemTemplate.Id > 0);

            Assert.Equal(required0, model_get.ItemTemplateVMs.Where(vm => vm.ItemTemplate.ItemName == itemName0).Single().ItemTemplate.IsRequired);
            Assert.Equal(required1, model_get.ItemTemplateVMs.Where(vm => vm.ItemTemplate.ItemName == itemName1).Single().ItemTemplate.IsRequired);
        }
    }

    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityEditView_ItemTemplate_Tests
    { 
        readonly ServiceProviderFixture _fixture;

        public CommunityEditView_ItemTemplate_Tests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Edit_append_item_templates()
        { 
            var (_, model_post) = await  _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            int commId = model_post.Id;

            // update item templates
            var model_update = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(commId, 0);

            string updated = ShortGuid.NewGuid();
            string appended = ShortGuid.NewGuid();
            string unchanged = model_update.ItemTemplateVMs[0].ItemTemplate.ItemName;

            model_update.ItemTemplateVMs[1].ItemTemplate.ItemName = updated;
            model_update.ItemTemplateVMs.Add(new MemberProfileItemTemplateVm { ItemTemplate = new MemberProfileItemTemplate { ItemName = appended } });

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
            await _fixture.GetServiceNewScope<ICommunityEditHandler>().PostAsync(model_update);

            // verify
            var editViewService = _fixture.GetServiceNewScope<ICommunityEditHandler>();
            var model_get = editViewService.Get(commId, 0);

            Assert.Equal(3, model_get.ItemTemplateVMs.Count);

            Assert.NotNull(model_get.ItemTemplateVMs.Where(vm => vm.ItemTemplate.ItemName == updated).SingleOrDefault());
            Assert.NotNull(model_get.ItemTemplateVMs.Where(vm => vm.ItemTemplate.ItemName == appended).SingleOrDefault());
            Assert.NotNull(model_get.ItemTemplateVMs.Where(vm => vm.ItemTemplate.ItemName == unchanged).SingleOrDefault());
        }

        [Fact]
        public async Task Add_more_item_templates()
        {
            var (_, model_post) = await  _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            int itemTemplateNumber = model_post.ItemTemplateVMs.Where(x => !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Count();

            // when addMore = 0
            var model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 0);
            Assert.Equal(itemTemplateNumber, model_get.ItemTemplateVMs.Count);

            // when addMore = 10
            model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 10);
            Assert.Equal(itemTemplateNumber + 10, model_get.ItemTemplateVMs.Count);
        }

        [Fact]
        public async Task Existing_item_templates_with_empty_names_shouldNot_be_saved()
        {
            var (_, model_post) = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();

            // make change
            var model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 0);
            string currentName_before = model_get.ItemTemplateVMs[0].ItemTemplate.ItemName;

            model_get.ItemTemplateVMs[0].ItemTemplate.ItemName = "   ";
            await _fixture.GetServiceNewScope<ICommunityEditHandler>().PostAsync(model_get);

            // verify
            var model_get_after = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 0);
            string currentName_after = model_get_after.ItemTemplateVMs[0].ItemTemplate.ItemName;

            Assert.Equal(currentName_before, currentName_after);
        }

        [Fact]
        public async Task New_item_templates_with_empty_names_shouldNot_be_saved()
        { 
            var (_, model_post) = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromViewService();
            int vmSaved = model_post.ItemTemplateVMs.Where(x => !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Count();

            // add more item templates with blank names
            var model_get = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_post.Id, 7);
            int vmCount = model_get.ItemTemplateVMs.Count;
            model_get.ItemTemplateVMs[vmCount - 1].ItemTemplate.ItemName = "  ";
            model_get.ItemTemplateVMs[vmCount - 2].ItemTemplate.ItemName = "";
            model_get.ItemTemplateVMs[vmCount - 3].ItemTemplate.ItemName = ShortGuid.NewGuid(); // only this should be saved
            await _fixture.GetServiceNewScope<ICommunityEditHandler>().PostAsync(model_get);

            // verify
            var model_get2 = _fixture.GetServiceNewScope<ICommunityEditHandler>().Get(model_get.Id, 0);
            int vmCount2 = model_get2.ItemTemplateVMs.Count;
            Assert.Equal(vmSaved + 1, vmCount2);
        }
    }

    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityMembersView_Tests
    {
        readonly ServiceProviderFixture _fixture;

        public CommunityMembersView_Tests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Delete_selected_members()
        {
            // create test community with members
            var community = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromRepository();

            // delete members
            var viewModel = _fixture.GetServiceNewScope<ICommunityMembersHandler>().Get(community.Id);
            int beforeDelete = viewModel.MemberItemVms.Count;

            viewModel.MemberItemVms[0].Selected = true;
            viewModel.MemberItemVms[1].Selected = true;
            viewModel.MemberItemVms[2].Selected = true;

            await _fixture.GetServiceNewScope<ICommunityMembersHandler>().PostToDeleteSelected(viewModel);

            // verify
            var viewModel2 = _fixture.GetServiceNewScope<ICommunityMembersHandler>().Get(community.Id);
            int afterDelete = viewModel2.MemberItemVms.Count;

            Assert.Equal(beforeDelete - 3, afterDelete);
        }
    }

    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityGroupView_Tests
    {
        readonly ServiceProviderFixture _fixture;

        public CommunityGroupView_Tests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Delete_selected_groups()
        {
            // create test community with groups
            var community = await _fixture.GetServiceNewScope<ICommunityTestDataProvider>().CreateTestCommunityFromRepository();

            //var groupRepo = _fixture.GetService<IRepositoryBase<Group>>();

            // delete groups
            var viewModel = _fixture.GetServiceNewScope<ICommunityGroupsHandler>().Get(community.Id);
            int beforeDelete = viewModel.ItemViewModels.Count;

            viewModel.ItemViewModels[0].Selected = true;
            viewModel.ItemViewModels[1].Selected = true;

            await _fixture.GetServiceNewScope<ICommunityGroupsHandler>().PostToDeleteSelected(viewModel);

            // vefiry
            var viewModel2 = _fixture.GetServiceNewScope<ICommunityGroupsHandler>().Get(community.Id);
            int afterDelete = viewModel2.ItemViewModels.Count;

            Assert.Equal(beforeDelete - 2, afterDelete);
        }
    }
}
