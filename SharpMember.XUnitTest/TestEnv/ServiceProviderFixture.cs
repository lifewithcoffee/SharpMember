using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SharpMember.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace U.TestEnv
{
    public class ServiceProviderFixture : IDisposable
    {

        IServiceProvider _serviceProvider = null;
        public IServiceProvider ServiceProvider { get { return _serviceProvider.CreateScope().ServiceProvider; } }

        public T GetServiceNewScope<T>()
        {
            return _serviceProvider.CreateScope().ServiceProvider.GetService<T>();
        }

        public ServiceProviderFixture()
        {
            string projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            string sharpMemberDir = Path.Combine(projectDir, "../SharpMember");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(sharpMemberDir)
                .AddJsonFile(TestGlobalSettings.sharpMemberJsonSettingName, optional: true, reloadOnChange: true)
                .AddUserSecrets(userSecretsId: "aspnet-SharpMember-4C3332C6-4145-4408-BDD4-63A97039ED0D") // use project SharpMember's secret id
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSharpMemberCore(configuration);
            serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose() { }
    }

    [CollectionDefinition(nameof(ServiceProviderCollection))]
    public class ServiceProviderCollection : ICollectionFixture<ServiceProviderFixture> { }
}
