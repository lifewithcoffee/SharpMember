using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharpMember.Core.Data.Models.Member;
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

        int count = 0;

        public void Count()
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
        void Count();
        int GetCount();
    }

    class TransientTest : ITransientTest
    {
        int count = 0;

        public void Count()
        {
            count++;
        }

        public int GetCount()
        {
            return count;
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

            // test transient injection
            var transientTest = serviceProvider.GetService<ITransientTest>();
            transientTest.Count();
            Assert.Equal(1, transientTest.GetCount());

            transientTest = serviceProvider.GetService<ITransientTest>();
            transientTest.Count();
            Assert.Equal(1, transientTest.GetCount());

            // test scoped injection
            var scopedTest = serviceProvider.GetService<IScopedTest>();
            scopedTest.Count();
            Assert.Equal(1, scopedTest.GetCount());

            scopedTest = serviceProvider.GetService<IScopedTest>();
            scopedTest.Count();
            Assert.Equal(2, scopedTest.GetCount());

            // test scoped injection again on a new scope
            var newScopedTest = serviceProvider.CreateScope().ServiceProvider.GetService<IScopedTest>();
            newScopedTest.Count();
            Assert.Equal(1, newScopedTest.GetCount());  // new scope counts from 0
            Assert.Equal(2, scopedTest.GetCount());     // old scope stays unchanged
        }
    }
}
