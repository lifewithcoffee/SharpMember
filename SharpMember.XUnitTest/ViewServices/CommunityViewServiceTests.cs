using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Views.ViewModels;
using System.Threading.Tasks;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;

namespace U.ViewServices
{
    public class CommunityViewServiceTests : DependencyEnabled
    {
        TestUtil util = new TestUtil();

        [Fact]
        public async Task Test_CreateViewService()
        {
            string appUserId = await util.GetExistingAppUserId(this.serviceProvider);
            string newCommunityName = Guid.NewGuid().ToString();
            string itemName1 = nameof(itemName1) + ' ' + Guid.NewGuid().ToString();
            string itemName2 = nameof(itemName2) + ' ' + Guid.NewGuid().ToString();
            int commId = 0;

            // post to create a new community
            {
                var createViewService = this.serviceProvider.GetService<ICommunityCreateViewService>();

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
                var editViewService = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityEditViewService>();
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
            string appUserId = await util.GetExistingAppUserId(this.serviceProvider);
            string itemName1 = nameof(itemName1) + ' ' + Guid.NewGuid().ToString();
            int commId = 0;

            // prepare to create an existing community
            {
                var createViewService = this.serviceProvider.GetService<ICommunityCreateViewService>();
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
                var editViewService = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityEditViewService>();
                var model = editViewService.Get(commId);

                model.MemberProfileItemTemplates[1].ItemName = updatedItem;
                model.MemberProfileItemTemplates.Add(new MemberProfileItemTemplate { ItemName = appendedItem });

                await editViewService.PostAsync(model);
            }

            // get the updated community to verify
            {
                var editViewService = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityEditViewService>();
                var model = editViewService.Get(commId);

                Assert.Equal(3, model.MemberProfileItemTemplates.Count);

                Assert.Equal(itemName1, model.MemberProfileItemTemplates[0].ItemName);
                Assert.Equal(updatedItem, model.MemberProfileItemTemplates[1].ItemName);
                Assert.Equal(appendedItem, model.MemberProfileItemTemplates[2].ItemName);
            }
        }
    }
}
