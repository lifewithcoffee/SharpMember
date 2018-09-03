using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Views.ViewModels;
using System.Threading.Tasks;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using U.TestEnv;

namespace U.ViewServices
{
    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityViewServiceTests
    {
        TestUtil util;
        ServiceProviderFixture _serviceProviderFixture;

        public CommunityViewServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
            this._serviceProviderFixture = serviceProviderFixture;
            util = new TestUtil(serviceProviderFixture.ServiceProvider);
        }

        [Fact]
        public async Task Test_CreateViewService()
        {
            string appUserId = await util.GetExistingAppUserId();
            string newCommunityName = Guid.NewGuid().ToString();
            string itemName1 = nameof(itemName1) + ' ' + Guid.NewGuid().ToString();
            string itemName2 = nameof(itemName2) + ' ' + Guid.NewGuid().ToString();
            int commId = 0;

            // post to create a new community
            {
                var createViewService = _serviceProviderFixture.GetServiceNewScope<ICommunityCreateViewService>();

                // get a new model
                CommunityUpdateVM model = createViewService.Get();
                Assert.Equal(0, model.Id);
                Assert.True(string.IsNullOrWhiteSpace(model.Name));
                Assert.Equal(5, model.MemberProfileItemTemplates.Count);    // initialize to have 5 empty item templates

                // modify and post model
                model.Name = newCommunityName;

                model.MemberProfileItemTemplates[0].ItemName = itemName1;
                model.MemberProfileItemTemplates[0].IsRequired = true;

                model.MemberProfileItemTemplates[1].ItemName = itemName2;
                model.MemberProfileItemTemplates[1].IsRequired = false;

                model.MemberProfileItemTemplates[2].ItemName = "  ";

                commId = await createViewService.PostAsync(appUserId, model);
                Assert.True(commId > 0);
            }

            // get the newly created community to verify
            {
                var editViewService = _serviceProviderFixture.GetServiceNewScope<ICommunityEditViewService>();

                var model = editViewService.Get(commId);
                Assert.Equal(commId, model.Id);
                Assert.Equal(newCommunityName, model.Name);

                Assert.Equal(2, model.MemberProfileItemTemplates.Count);

                Assert.Equal(commId, model.MemberProfileItemTemplates[0].CommunityId);
                Assert.True(model.MemberProfileItemTemplates[0].IsRequired);

                Assert.Equal(commId, model.MemberProfileItemTemplates[1].CommunityId);
                Assert.False(model.MemberProfileItemTemplates[1].IsRequired);

                Assert.True(model.MemberProfileItemTemplates[0].Id > 0);
                Assert.True(model.MemberProfileItemTemplates[1].Id > 0);

                Assert.Equal(itemName1, model.MemberProfileItemTemplates[0].ItemName);
                Assert.Equal(itemName2, model.MemberProfileItemTemplates[1].ItemName);
            }

        }

        [Fact]
        public async Task Test_EditViewService()
        { 
            string appUserId = await util.GetExistingAppUserId();
            string itemName1 = nameof(itemName1) + ' ' + Guid.NewGuid().ToString();
            int commId = 0;

            // prepare to create an existing community
            {
                var createViewService = _serviceProviderFixture.GetServiceNewScope<ICommunityCreateViewService>();
                CommunityUpdateVM model = createViewService.Get();

                model.Name = Guid.NewGuid().ToString();

                model.MemberProfileItemTemplates[0].ItemName = itemName1;
                model.MemberProfileItemTemplates[1].ItemName = Guid.NewGuid().ToString();

                commId = await createViewService.PostAsync(appUserId, model);
            }

            string updatedItem = nameof(updatedItem) + ' ' + Guid.NewGuid().ToString();
            string appendedItem = nameof(appendedItem) + ' ' + Guid.NewGuid().ToString();

            // update item templates
            {
                /**
                 * read and do changes
                 */
                var editViewService_read = _serviceProviderFixture.GetServiceNewScope<ICommunityEditViewService>();
                var model = editViewService_read.Get(commId);

                model.MemberProfileItemTemplates[1].ItemName = updatedItem;
                model.MemberProfileItemTemplates.Add(new MemberProfileItemTemplate { ItemName = appendedItem });

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
                var editViewService_write = _serviceProviderFixture.GetServiceNewScope<ICommunityEditViewService>();
                await editViewService_write.PostAsync(model);
            }

            // get the updated community to verify
            {
                var editViewService = _serviceProviderFixture.GetServiceNewScope<ICommunityEditViewService>();
                var model = editViewService.Get(commId);

                Assert.Equal(3, model.MemberProfileItemTemplates.Count);

                Assert.Equal(itemName1, model.MemberProfileItemTemplates[0].ItemName);
                Assert.Equal(updatedItem, model.MemberProfileItemTemplates[1].ItemName);
                Assert.Equal(appendedItem, model.MemberProfileItemTemplates[2].ItemName);
            }
        }
    }
}
