using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
    public class OrganizationTests
    {
        [Fact]
        public void TestAdd()
        {
            string configureFileDir = $"TestGlobalSettings\\SharpMember";
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(TestGlobalSettings.sharpMemberProjectPath)
                .AddJsonFile(TestGlobalSettings.sharpMemberJsonSettingName)
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSharpMemberCore(Configuration);
            serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);   // TODO: make an MCN notes here

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IOrganizationRepository repo = serviceProvider.GetService<IOrganizationRepository>();

            Assert.NotNull(repo);

            repo.Add(new Organization { Name = Guid.NewGuid().ToString()});
            repo.Commit();
        }
    }
}
