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
                await _communityService.AddMemberAsync("", ShortGuid.NewGuid(), "", "");

            // add member profile templates (5 required, 3 optional)
            for (int i = 0; i < 5; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), true);

            for (int i = 0; i < 3; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), false);

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
        [Fact]
        public void Create_test_community()
        {

        }
    }
}
