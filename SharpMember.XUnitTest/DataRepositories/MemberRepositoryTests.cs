using SharpMember.Core.Data.DataServices.MemberSystem;
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
using U.TestEnv;
using NetCoreUtils.Database;

namespace U.DataRepositories
{
    [Collection(nameof(ServiceProviderCollection))]
    public class MemberRepositoryTests
    {
        readonly ServiceProviderFixture _fixture;

        public MemberRepositoryTests(ServiceProviderFixture serviceProviderFixture)
        {
            _fixture = serviceProviderFixture;
        }

        [Fact]
        public void Add_with_invalide_communityId_should_throw_exception()
        {
            int nonexistentOrgId = _fixture.Util.GetNonexistentCommunityId();

            IMemberService repo = _fixture.GetServiceNewScope<IMemberService>();
            Assert.Throws<CommunityNotExistsException>(() => repo.Add(new Member { CommunityId = nonexistentOrgId }));
        }

        [Fact]
        public async Task Test_assign_member_number()
        {
            int existingOrgId = _fixture.Util.GetExistingCommunityId();

            IMemberService repo = _fixture.GetServiceNewScope<IMemberService>();

            var member1 = repo.Add(new Member { CommunityId = existingOrgId });
            var member2 = repo.Add(new Member { CommunityId = existingOrgId });
            await repo.Repo.CommitAsync();

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
            int existingCommunityId = _fixture.Util.GetExistingCommunityId();
            string[] originalTemplats = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            var itemTemplateRepo = _fixture.GetServiceNewScope<IMemberProfileItemTemplateService>();
            await itemTemplateRepo.AddTemplatesAsync(existingCommunityId, originalTemplats, true);
            await itemTemplateRepo.CommitAsync();

            // Generate & verify a new member
            {
                var memberRepo = _fixture.GetServiceNewScope<IMemberService>();
                var newMember = await memberRepo.GenerateNewMemberWithProfileItemsAsync(existingCommunityId, Guid.NewGuid().ToString());
                Assert.Equal(0, newMember.MemberNumber);
                Assert.Equal(2, newMember.MemberProfileItems.Count);
            }

            // Delete one item template
            {
                var itemTemplateRepo2 = _fixture.GetServiceNewScope<IMemberProfileItemTemplateService>();
                var templateToBeDeleted = itemTemplateRepo2.GetByCommunityId(existingCommunityId).First();

                var itemTemplateRepo3 = _fixture.GetServiceNewScope<IRepository<MemberProfileItemTemplate>>();
                itemTemplateRepo3.Remove(templateToBeDeleted);
                await itemTemplateRepo3.CommitAsync();
            }

            // Generate & verify a new member after deletion
            {
                var memberRepo = _fixture.GetServiceNewScope<IMemberService>();
                var newMember = await memberRepo.GenerateNewMemberWithProfileItemsAsync(existingCommunityId, Guid.NewGuid().ToString());
                Assert.Single(newMember.MemberProfileItems);
            }
        }
    }
}
