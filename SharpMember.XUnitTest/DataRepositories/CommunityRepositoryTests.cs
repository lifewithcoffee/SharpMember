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
    public class CommunityTests: DependencyEnabled
    {
        [Fact]
        public void TestAddGetUpdateCommunity()
        {
            ICommunityRepository repo = this.serviceProvider.GetService<ICommunityRepository>();
            Assert.NotNull(repo);

            // add
            var newCommunity = repo.Add(new Community { Name = Guid.NewGuid().ToString()});
            Assert.False(newCommunity.Id > 0);

            repo.Commit();
            Assert.True(newCommunity.Id > 0);

            // read to verify add
            var readRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityRepository>();
            var readCommunity = readRepo.GetById(newCommunity.Id);
            Assert.Equal(newCommunity.Name, readCommunity.Name);

            // update
            var updateRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityRepository>();
            var orgBeforeUpdate = updateRepo.GetById(newCommunity.Id);

            string newCommunityName = Guid.NewGuid().ToString();
            orgBeforeUpdate.Name = newCommunityName;
            updateRepo.Commit();

            // read to verify update
            var readRepo2 = this.serviceProvider.CreateScope().ServiceProvider.GetService<ICommunityRepository>();
            var orgAfterUpdate = readRepo2.GetById(newCommunity.Id);
            Assert.Equal(newCommunityName, orgAfterUpdate.Name);
        }
    }
}
