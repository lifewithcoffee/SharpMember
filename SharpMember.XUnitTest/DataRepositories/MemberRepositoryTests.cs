using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;
using Xunit.Abstractions;
using SharpMember.Core;
using SharpMember.Core.Definitions;

namespace U.DataRepositories
{
    public class MemberRepositoryTests: DependencyEnabled
    {
        private TestUtil util = new TestUtil();

        [Fact]
        public void Add_with_invalide_communityId_should_throw_exception()
        {
            int nonexistentOrgId = util.GetNonexistentCommunityId();

            IMemberRepository repo = this.serviceProvider.GetService<IMemberRepository>();
            Assert.Throws<CommunityNotExistsException>(() => repo.Add(new Member { CommunityId = nonexistentOrgId }));
        }

        [Fact]
        public async Task Test_AssignMemberNubmer()
        {
            int existingOrgId = util.GetExistingCommunityId();

            IMemberRepository repo = this.serviceProvider.GetService<IMemberRepository>();

            var member1 = repo.Add(new Member { CommunityId = existingOrgId });
            var member2 = repo.Add(new Member { CommunityId = existingOrgId });
            await repo.CommitAsync();

            int nextMemberNumber = repo.GetNextUnassignedMemberNumber(existingOrgId);
            int result1 = await repo.AssignMemberNubmerAsync(member1.Id, nextMemberNumber);
            Assert.Equal(nextMemberNumber, result1);

            int result2 = await repo.AssignMemberNubmerAsync(member2.Id, nextMemberNumber);
            Assert.True(result2 > nextMemberNumber);
        }

        [Fact]
        public async Task Community_MemberProfileItemTemplate_change_should_cause_new_member_profile_item_change()
        {
            // create an community and the relevant member item templates
            int existingCommunityId = util.GetExistingCommunityId();
            string[] originalTemplats = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            var itemTemplateRepo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            await itemTemplateRepo.AddRquiredTemplatesAsync(existingCommunityId, originalTemplats );
            await itemTemplateRepo.CommitAsync();

            // Generate & verify a new member
            {
                var memberRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
                var newMember = await memberRepo.GenerateNewMemberWithProfileItemsAsync(existingCommunityId, Guid.NewGuid().ToString());
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
                var templateToBeDeleted = itemTemplateRepo2.GetByCommunityId(existingCommunityId).First();
                itemTemplateRepo2.Delete(templateToBeDeleted);
                await itemTemplateRepo2.CommitAsync();
            }

            // Generate & verify a new member after deletion
            {
                var memberRepo2 = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
                var newMember2 = await memberRepo2.GenerateNewMemberWithProfileItemsAsync(existingCommunityId, Guid.NewGuid().ToString());
                Assert.Equal(1, newMember2.MemberProfileItems.Count);
            }
        }
    }
}
