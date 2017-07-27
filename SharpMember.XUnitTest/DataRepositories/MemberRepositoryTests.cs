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
        public void Test_AssignMemberNubmer()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Organization_MemberProfileItemTemplate_change_should_cause_new_member_profile_item_change()
        {
            // create an organization and the relevant member item templates
            int existingOrgId = util.GetExistingOrganizationId();
            string[] originalTemplats = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            var itemTemplateRepo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            await itemTemplateRepo.AddRquiredTemplatesAsync(existingOrgId, originalTemplats );
            await itemTemplateRepo.CommitAsync();

            // Generate & verify a new member
            {
                var memberRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
                var newMember = await memberRepo.GenerateNewMemberAsync(existingOrgId);
                Assert.Equal(0, newMember.MemberNumber);
                Assert.Equal(originalTemplats.Length, newMember.MemberProfileItems.Count);
                foreach (var item in newMember.MemberProfileItems)
                {
                    Assert.True(originalTemplats.Contains(item.ItemName));
                }
            }

            // Delete one item template
            {
                var itemTemplateRepo2 = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberProfileItemTemplateRepository>();
                var templateToBeDeleted = itemTemplateRepo2.GetByOrganizationId(existingOrgId).First();
                itemTemplateRepo2.Delete(templateToBeDeleted);
                await itemTemplateRepo2.CommitAsync();
            }

            // Generate & verify a new member after deletion
            {
                var memberRepo2 = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
                var newMember2 = await memberRepo2.GenerateNewMemberAsync(existingOrgId);
                Assert.Equal(1, newMember2.MemberProfileItems.Count);
            }
        }
    }
}
