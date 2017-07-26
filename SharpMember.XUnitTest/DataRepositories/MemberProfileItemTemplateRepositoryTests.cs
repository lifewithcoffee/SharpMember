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

namespace U.DataRepositories
{
    class LocalUtil: DependencyEnabled
    {
        public int GetExistingOrganizationId()
        {
            var repo = this.serviceProvider.GetService<IOrganizationRepository>();
            var org = repo.Add(Guid.NewGuid().ToString());
            repo.Commit();
            return org.Id;
        }

        public int GetNonExistingOrganizationId()
        {
            var repo = this.serviceProvider.GetService<IOrganizationRepository>();
            var org = repo.GetAll().OrderBy(o => o.Id).LastOrDefault();
            if(org == null)
            {
                return new Random().Next(); // return non-negative integer
            }
            else
            {
                return org.Id + new Random().Next();
            }
        }
    }

    public class MemberProfileItemTemplateRepositoryTests: DependencyEnabled
    {
        LocalUtil util = new LocalUtil();

        [Fact]
        public void TestAddGetUpdateMemberProfileItemTemplate()
        {
            int existingOrgId = this.util.GetExistingOrganizationId();
            string[] itemNames = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            // add
            var repo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            foreach(var name in itemNames)
            {
                repo.AddWithExceptionAsync(existingOrgId, name);
            }
            repo.Commit();

            // read
            throw new NotImplementedException();
        }

        [Fact]
        public async Task TestAddToNonExistOrganizationId()
        {
            int nonExistingOrgId = this.util.GetNonExistingOrganizationId();
            var repo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            OrganizationNotExistException ex = await Assert.ThrowsAsync<OrganizationNotExistException>(() => repo.AddWithExceptionAsync(nonExistingOrgId, Guid.NewGuid().ToString()));
            Assert.Equal($"The organization with Id {nonExistingOrgId} does not exist.", ex.Message);
        }
    }
}
