using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Data.Repositories.MemberSystem;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Definitions;

namespace U.DataRepositories
{
    public class MemberProfileItemTemplateRepositoryTests: DependencyEnabled
    {
        TestUtil util = new TestUtil();

        [Fact]
        public async Task Test_add_update_delete_MemberProfileItemTemplate()
        {
            int existingOrgId = this.util.GetExistingCommunityId();
            string[] itemNames = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            // ======================================================================================
            // add
            var repo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            foreach(var name in itemNames)
            {
                await repo.AddTemplateAsync(existingOrgId, name, false);
            }
            await repo.CommitAsync();

            // verify add
            var repoOrg = this.serviceProvider.GetService<ICommunityRepository>();
            var readItemNames = repoOrg.GetMany(o => o.Id == existingOrgId)
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
            var repoUpdate = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            var updated = repoUpdate.GetMany(t => t.CommunityId == existingOrgId).First();
            string newItemName = $"updated-{Guid.NewGuid().ToString()}";
            updated.ItemName = newItemName;
            repoUpdate.Commit();

            // verify upate
            var repoRead = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            var updateItemNames = repoUpdate.GetMany(t => t.CommunityId == existingOrgId).Select(t => t.ItemName).ToList();
            Assert.Equal(2, updateItemNames.Count());
            Assert.Contains(newItemName, updateItemNames);

            // ======================================================================================
            // delete
            var repoDelete = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            var deleteTarget = repoDelete.GetMany(t => t.CommunityId == existingOrgId).Last();
            repoDelete.Delete(deleteTarget);
            repoDelete.Commit();

            // verify delete
            var repoRead2 = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            var remained = repoRead2.GetMany(t => t.CommunityId == existingOrgId).ToList();
            Assert.Single(remained);
        }

        [Fact]
        public async Task Add_with_nonexitent_CommunityId_should_throw_exception()
        {
            int nonExistentOrgId = this.util.GetNonexistentCommunityId();
            var repo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            CommunityNotExistsException ex = await Assert.ThrowsAsync<CommunityNotExistsException>(() => repo.AddTemplateAsync(nonExistentOrgId, Guid.NewGuid().ToString(), false));
            Assert.Equal($"The community with Id {nonExistentOrgId} does not exist.", ex.Message);
        }
    }
}
