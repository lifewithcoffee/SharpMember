using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Data.Models.MemberSystem;

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
    }
}
