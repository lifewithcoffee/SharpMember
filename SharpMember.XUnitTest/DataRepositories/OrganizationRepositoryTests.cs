using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SharpMember.Core;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace U.DataRepositories
{
    public class OrganizationTests: DependencyEnabled
    {
        [Fact]
        public void TestAddGetUpdateOrganization()
        {
            IOrganizationRepository repo = this.serviceProvider.GetService<IOrganizationRepository>();
            Assert.NotNull(repo);

            // add
            var newOrganization = repo.Add(new Organization { Name = Guid.NewGuid().ToString()});
            Assert.False(newOrganization.Id > 0);

            repo.Commit();
            Assert.True(newOrganization.Id > 0);

            // read to verify add
            var readRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IOrganizationRepository>();
            var readOrganization = readRepo.GetById(newOrganization.Id);
            Assert.Equal(newOrganization.Name, readOrganization.Name);

            // update
            var updateRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IOrganizationRepository>();
            var orgBeforeUpdate = updateRepo.GetById(newOrganization.Id);

            string newOrganizationName = Guid.NewGuid().ToString();
            orgBeforeUpdate.Name = newOrganizationName;
            updateRepo.Commit();

            // read to verify update
            var readRepo2 = this.serviceProvider.CreateScope().ServiceProvider.GetService<IOrganizationRepository>();
            var orgAfterUpdate = readRepo2.GetById(newOrganization.Id);
            Assert.Equal(newOrganizationName, orgAfterUpdate.Name);
        }
    }
}
