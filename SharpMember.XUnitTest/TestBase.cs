using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SharpMember.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace U
{
    public class TestBase
    {
        protected IServiceProvider serviceProvider;

        public TestBase()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(TestGlobalSettings.sharpMemberProjectPath)
                .AddJsonFile(TestGlobalSettings.sharpMemberJsonSettingName)
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSharpMemberCore(Configuration);
            serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
