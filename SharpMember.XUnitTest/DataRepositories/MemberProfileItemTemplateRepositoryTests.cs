using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Data.DataServices.MemberSystem;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Models.MemberSystem;
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
        ServiceProviderFixture _fixture;

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
            var repo = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            foreach(var name in itemNames)
            {
                await repo.AddTemplateAsync(existingOrgId, name, false);
            }
            await repo.Repo.CommitAsync();

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
            var repoUpdate = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            var updated = repoUpdate.Repo.Query(t => t.CommunityId == existingOrgId).First();
            string newItemName = $"updated-{Guid.NewGuid().ToString()}";
            updated.ItemName = newItemName;
            repoUpdate.Repo.Commit();

            // verify upate
            var repoRead = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            var updateItemNames = repoUpdate.Repo.Query(t => t.CommunityId == existingOrgId).Select(t => t.ItemName).ToList();
            Assert.Equal(2, updateItemNames.Count());
            Assert.Contains(newItemName, updateItemNames);

            // ======================================================================================
            // delete
            var repoDelete = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            var deleteTarget = repoDelete.Repo.Query(t => t.CommunityId == existingOrgId).Last();
            repoDelete.Repo.Remove(deleteTarget);
            repoDelete.Repo.Commit();

            // verify delete
            var repoRead2 = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            var remained = repoRead2.Repo.Query(t => t.CommunityId == existingOrgId).ToList();
            Assert.Single(remained);
        }

        [Fact]
        public async Task Add_with_nonexitent_CommunityId_should_throw_exception()
        {
            int nonExistentOrgId = _fixture.Util.GetNonexistentCommunityId();
            var repo = _fixture.GetServiceNewScope<IMemberProfileItemTemplateRepository>();
            CommunityNotExistsException ex = await Assert.ThrowsAsync<CommunityNotExistsException>(() => repo.AddTemplateAsync(nonExistentOrgId, Guid.NewGuid().ToString(), false));
            Assert.Equal($"The community with Id {nonExistentOrgId} does not exist.", ex.Message);
        }
    }
}
