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
    public class DependencyEnabled
    {
        protected IServiceProvider serviceProvider;

        public DependencyEnabled()
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
