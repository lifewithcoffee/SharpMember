using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace U.DataRepositories
{
    public class MemberRepositoryTests: DependencyEnabled
    {
        TestUtil util = new TestUtil();

        [Fact]
        public async Task TestOrganization_MemberProfileItemTemplate_change_should_cause_new_member_profile_item_change()
        {
            // create an organization and the relevant member item templates
            int existingOrgId = util.GetExistingOrganizationId();
            string[] originalTemplats = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            var itemTemplateRepo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            await itemTemplateRepo.AddRquiredTemplatesAsync(existingOrgId, originalTemplats );
            await itemTemplateRepo.CommitAsync();

            // Generate a new member
            var memberRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
            var newMember = await memberRepo.GenerateNewMemberAsync(existingOrgId);
            Assert.Equal(0, newMember.MemberNumber);
            Assert.Equal(originalTemplats.Length, newMember.MemberProfileItems.Count);
            foreach(var item in newMember.MemberProfileItems)
            {
                Assert.True(originalTemplats.Contains(item.ItemName));
            }
        }
    }
}
