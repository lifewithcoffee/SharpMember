using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Threading.Tasks;

namespace U.DataRepositories
{
    public class MemberProfileItemRepositoryTests: DependencyEnabled
    {
        TestUtil util = new TestUtil();

        [Fact]
        public void Test_UpdateProfile()
        {
            int existingMemberId = this.util.GetExistingMemberId();

            // create profile
            {
                var repo = this.serviceProvider.GetService<IMemberProfileItemRepository>();

                var profile = new List<MemberProfileItem>
                {
                    new MemberProfileItem { IsRequired = false, ItemName = "name1", ItemValue="value1", MemberId = existingMemberId },
                    new MemberProfileItem { IsRequired = true, ItemName = "name2", ItemValue="value2", MemberId = existingMemberId },
                    new MemberProfileItem { IsRequired = false, ItemName = "name3", ItemValue="value3", MemberId = existingMemberId },
                    new MemberProfileItem { IsRequired = true, ItemName = "name4", ItemValue="value4", MemberId = existingMemberId },
                };

                repo.UpdateProfile(existingMemberId, profile);
                repo.Commit();
            }

            // verify profile creation
            {
                var repo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemRepository>();
                List<MemberProfileItem> profile = repo.GetMany(i => i.MemberId == existingMemberId).ToList();
                Assert.Equal(4, profile.Count());

                var item1 = profile.Where(i => i.ItemName == "name1").Single();
                Assert.Equal(false, item1.IsRequired);
                Assert.Equal("value1", item1.ItemValue);

                var item2 = profile.Where(i => i.ItemName == "name2").Single();
                Assert.Equal(true, item2.IsRequired);
                Assert.Equal("value2", item2.ItemValue);

                var item3 = profile.Where(i => i.ItemName == "name3").Single();
                Assert.Equal(false, item3.IsRequired);
                Assert.Equal("value3", item3.ItemValue);

                var item4 = profile.Where(i => i.ItemName == "name4").Single();
                Assert.Equal(true, item4.IsRequired);
                Assert.Equal("value4", item4.ItemValue);
            }

            // modify profile
            {
                var repo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemRepository>();
                List<MemberProfileItem> profile = repo.GetMany(i => i.MemberId == existingMemberId).ToList();

                // remove one item
                profile.Remove(profile.Where(i => i.ItemName == "name1").Single());
                profile.Remove(profile.Where(i => i.ItemName == "name4").Single());

                // update one item
                var item = profile.Where(i => i.ItemName == "name2").Single();
                item.IsRequired = false;
                item.ItemValue = "value2 modified";

                // add one item
                profile.Add(new MemberProfileItem { IsRequired = false, ItemName = "name5", ItemValue = "value5", MemberId = existingMemberId});
                repo.UpdateProfile(existingMemberId, profile);
                repo.Commit();
            }

            // verify profile modification
            {
                var repo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemRepository>();
                List<MemberProfileItem> profile = repo.GetMany(i => i.MemberId == existingMemberId).ToList();
                Assert.Equal(3, profile.Count());

                var item2 = profile.Where(i => i.ItemName == "name2").Single(); // the modified one
                Assert.Equal(false, item2.IsRequired);
                Assert.Equal("value2 modified", item2.ItemValue);

                var item3 = profile.Where(i => i.ItemName == "name3").Single(); // unchanged
                Assert.Equal(false, item3.IsRequired);
                Assert.Equal("value3", item3.ItemValue);

                var item5 = profile.Where(i => i.ItemName == "name5").Single(); // unchanged
                Assert.Equal(false, item5.IsRequired);
                Assert.Equal("value5", item5.ItemValue);
            }
        }


        [Fact]
        public async Task Test_GetByItemValue()
        {
            // create a community and the relevant member item templates
            int existingOrgId = util.GetExistingCommunityId();
            string[] originalTemplats = { "Item1", "Item2" };

            var itemTemplateRepo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            await itemTemplateRepo.AddTemplatesAsync(existingOrgId, originalTemplats, true);
            await itemTemplateRepo.CommitAsync();

            // create members with profile
            int memberId1, memberId2;
            {
                string appUserId = await util.GetExistingAppUserId(this.serviceProvider);
                var memberRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
                var newMember1 = await memberRepo.GenerateNewMemberWithProfileItemsAsync(existingOrgId, appUserId);
                var newMember2 = await memberRepo.GenerateNewMemberWithProfileItemsAsync(existingOrgId, appUserId);

                newMember1.MemberProfileItems = new List<MemberProfileItem> {
                    new MemberProfileItem { ItemName = "name1", ItemValue = "value1" },
                    new MemberProfileItem { ItemName = "name2", ItemValue = "value2" },
                };

                newMember2.MemberProfileItems = new List<MemberProfileItem> {
                    new MemberProfileItem { ItemName = "name2", ItemValue = "value2" },
                    new MemberProfileItem { ItemName = "name3", ItemValue = "value3" },
                    new MemberProfileItem { ItemName = "name4", ItemValue = "value4" },
                };

                memberRepo.Add(newMember1);
                memberRepo.Add(newMember2);

                await memberRepo.CommitAsync();

                memberId1 = newMember1.Id;
                memberId2 = newMember2.Id;
            }

            // verify member creation
            {
                var memberProfileItemRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemRepository>();
                Assert.Equal(2, memberProfileItemRepo.GetByMemberId(memberId1).Count());
                Assert.Equal(3, memberProfileItemRepo.GetByMemberId(memberId2).Count());
            }

            // Test GetByItemValue() method
            {
                var repo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemRepository>();
                Assert.Equal(1, repo.GetByItemValueContains(existingOrgId, "value1").ToList().Count());
                Assert.Equal(2, repo.GetByItemValueContains(existingOrgId, "value2").ToList().Count());
                Assert.Equal(1, repo.GetByItemValueContains(existingOrgId, "value3").ToList().Count());
                Assert.Equal(1, repo.GetByItemValueContains(existingOrgId, "value4").ToList().Count());
                Assert.Equal(5, repo.GetByItemValueContains(existingOrgId, "value").ToList().Count());    // partial value
            }
        }
    }
}
