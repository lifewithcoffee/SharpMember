using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace U.Misc
{
    interface IScopedTest
    {
        void Count();
        int GetCount();
    }

    class ScopedTest : IScopedTest
    {
        IServiceProvider _serviceProvider;

        int count = 0;

        public ScopedTest(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        void IScopedTest.Count()
        {
            count++;
        }

        public int GetCount()
        {
            return count;
        }
    }

    interface ITransientTest
    {
        void CountScopedTestUsingInjectedInstance();
        void CountScopedTestViaServiceProvider();
    }

    class TransientTest : ITransientTest
    {
        IServiceProvider _serviceProvider;
        IScopedTest _scopedTest;

        public TransientTest(IServiceProvider serviceProvider, IScopedTest scopedTest)
        {
            _serviceProvider = serviceProvider;
            _scopedTest = scopedTest;
        }

        public void CountScopedTestUsingInjectedInstance()
        {
            _scopedTest.Count();
        }

        public void CountScopedTestViaServiceProvider()
        {
            _serviceProvider.GetService<IScopedTest>().Count();
        }
    }

    public class BuildinDiTests
    {
        [Fact]
        public void DoTest()
        {
            // setup DI config
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ITransientTest, TransientTest>();
            serviceCollection.AddScoped<IScopedTest, ScopedTest>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // initial state
            var scopedTest = serviceProvider.GetService<IScopedTest>();
            Assert.Equal(0, scopedTest.GetCount());

            // both CountScopedTestUsingInjectedInstance() & CountScopedTestViaServiceProvider()
            // should apply to the same IScopedTest instance
            var transientTest = serviceProvider.GetService<ITransientTest>();
            transientTest.CountScopedTestUsingInjectedInstance();
            Assert.Equal(1, scopedTest.GetCount());

            transientTest.CountScopedTestViaServiceProvider();
            Assert.Equal(2, scopedTest.GetCount());

            // do the above test again on a new scope
            transientTest = serviceProvider.CreateScope().ServiceProvider.GetService<ITransientTest>();
            transientTest.CountScopedTestUsingInjectedInstance();
            transientTest.CountScopedTestViaServiceProvider();
            Assert.Equal(2, scopedTest.GetCount()); // the count should stay unchanged
        }
    }
}
