using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Data.DataServices.MemberSystem;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Models.Member;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Definitions;
using U.TestEnv;
using NetCoreUtils.Database;

namespace U.DataRepositories
{
    [Collection(nameof(ServiceProviderCollection))]
    public class MemberProfileItemTemplateRepositoryTests
    {
        readonly ServiceProviderFixture _fixture;

        public MemberProfileItemTemplateRepositoryTests(ServiceProviderFixture serviceProviderFixture)
        {
            _fixture = serviceProviderFixture;
        }

        [Fact]
        public async Task Test_add_update_delete_MemberProfileItemTemplate()
        {
            int existingOrgId = _fixture.Util.GetExistingCommunityId();
            Assert.True(existingOrgId > 0);
            string[] itemNames = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            // ======================================================================================
            // add
            var repo = _fixture.GetServiceNewScope<IMemberProfileItemTemplateService>();
            foreach(var name in itemNames)
            {
                await repo.AddTemplateAsync(existingOrgId, name, false);
            }
            await repo.CommitAsync();

            // verify add
            var repoOrg = _fixture.GetServiceNewScope<IRepository<Community>>();
            var readItemNames = repoOrg.Query(o => o.Id == existingOrgId)
                .Include(o => o.MemberProfileItemTemplates)
                .SelectMany(o => o.MemberProfileItemTemplates)
                .Select(t => t.ItemName)
                .ToList();

            Assert.Equal(2, readItemNames.Count);
            foreach(var name in itemNames)
            {
                Assert.Contains(name, readItemNames);
            }

            // ======================================================================================
            // update
            var repoUpdate = _fixture.GetServiceNewScope<IRepository<MemberProfileItemTemplate>>();
            var updated = repoUpdate.Query(t => t.CommunityId == existingOrgId).First();
            string newItemName = $"updated-{Guid.NewGuid().ToString()}";
            updated.ItemName = newItemName;
            repoUpdate.Commit();

            // verify upate
            var repoRead = _fixture.GetServiceNewScope<IRepository<MemberProfileItemTemplate>>();
            var updateItemNames = repoUpdate.Query(t => t.CommunityId == existingOrgId).Select(t => t.ItemName).ToList();
            Assert.Equal(2, updateItemNames.Count());
            Assert.Contains(newItemName, updateItemNames);

            // ======================================================================================
            // delete
            var repoDelete = _fixture.GetServiceNewScope<IRepository<MemberProfileItemTemplate>>();
            var deleteTarget = repoDelete.Query(t => t.CommunityId == existingOrgId).Last();
            repoDelete.Remove(deleteTarget);
            repoDelete.Commit();

            // verify delete
            var repoRead2 = _fixture.GetServiceNewScope<IRepository<MemberProfileItemTemplate>>();
            var remained = repoRead2.Query(t => t.CommunityId == existingOrgId).ToList();
            Assert.Single(remained);
        }

        [Fact]
        public async Task Add_with_nonexitent_CommunityId_should_throw_exception()
        {
            int nonExistentOrgId = _fixture.Util.GetNonexistentCommunityId();
            var repo = _fixture.GetServiceNewScope<IMemberProfileItemTemplateService>();
            CommunityNotExistsException ex = await Assert.ThrowsAsync<CommunityNotExistsException>(() => repo.AddTemplateAsync(nonExistentOrgId, Guid.NewGuid().ToString(), false));
            Assert.Equal($"The community with Id {nonExistentOrgId} does not exist.", ex.Message);
        }
    }
}
