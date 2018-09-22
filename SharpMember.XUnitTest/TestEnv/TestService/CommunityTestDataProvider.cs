using NetCoreUtils.String;
using SharpMember.Core.Data.Models.MemberSystem;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using SharpMember.Core.Data.Repositories.MemberSystem;
using NetCoreUtils.Database;
using SharpMember.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace U.TestEnv.TestService
{
    class CommunityTestDataProvider
    {
        TestUtil util;
        IServiceProvider _serviceProvider;

        public CommunityTestDataProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            util = new TestUtil(serviceProvider);
        }

        public async Task<Community> CreateTestCommunity()
        {
            var _communityService = _serviceProvider.GetService<ICommunityService>();

            string appUserId = await util.GetExistingAppUserId();
            await _communityService.CreateCommunityAsync(appUserId, ShortGuid.NewGuid());

            // add 10 members
            for (int i = 0; i < 10; i++)
                await _communityService.AddMemberAsync(null, ShortGuid.NewGuid(), "", "");

            // add member profile templates (5 required, 3 optional)
            for (int i = 0; i < 5; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), true);

            for (int i = 0; i < 3; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), false);

            await _communityService.CommitAsync();
            //_serviceProvider.GetService<IUnitOfWork<ApplicationDbContext>>().Commit();

            return _communityService.Community;
        }

        //public async Task<Community> CreateTestCommunity()
        //{
        //    string appUserId = await util.GetExistingAppUserId();
        //    string newCommunityName = ShortGuid.NewGuid();
        //    string itemName1 = nameof(itemName1) + ' ' + ShortGuid.NewGuid();
        //    string itemName2 = nameof(itemName2) + ' ' + ShortGuid.NewGuid();

        //    return null;
        //}
    }

    [Collection(nameof(ServiceProviderCollection))]
    public class CommunityTestDataProviderTests
    {
        ServiceProviderFixture _fixture;

        public CommunityTestDataProviderTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Create_test_community()
        {
            var communityTestDataProvider = new CommunityTestDataProvider(_fixture.ServiceProvider);
            var community = await communityTestDataProvider.CreateTestCommunity();

            community = _fixture.GetServiceNewScope<ICommunityRepository>()
                                .GetMany(c => c.Id == community.Id)
                                .Include(c => c.Members)
                                .Include(c => c.MemberProfileItemTemplates)
                                .Single();

            Assert.True(community.Id > 0);
            Assert.Equal(11, community.Members.Count());    // 1 owner self's and 10 new added
            Assert.Equal(8, community.MemberProfileItemTemplates.Count());
        }
    }
}
