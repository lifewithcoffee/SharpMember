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
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using SharpMember.Core.Views.ViewModels;

namespace U.TestEnv.TestService
{
    interface ICommunityTestDataProvider
    {
        Task<Community> CreateTestCommunityFromRepository();
        Task<CommunityUpdateVM> CreateTestCommunityFromViewService();
    }

    class CommunityTestDataProvider : ICommunityTestDataProvider
    {
        TestUtil util;
        IServiceProvider _serviceProvider;

        public CommunityTestDataProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            util = new TestUtil(serviceProvider);
        }

        /// <summary>
        /// Unit test: <see cref="CommunityTestDataProviderTests.Create_test_community_from_repository"/>
        /// </summary>
        public async Task<Community> CreateTestCommunityFromRepository()
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

            return _communityService.Community;
        }

        /// <summary>
        /// Unit test: <see cref="ViewServices.CommunityViewServiceTests.Community_create_view_post"/>
        /// </summary>
        public async Task<CommunityUpdateVM> CreateTestCommunityFromViewService()
        {
            var _vs = _serviceProvider.GetService<ICommunityCreateViewService>();

            string appUserId = await util.GetExistingAppUserId();

            CommunityUpdateVM model = _vs.Get();
            model.Name = ShortGuid.NewGuid();

            var template0 = model.MemberProfileItemTemplates[0];
            template0.ItemName = ShortGuid.NewGuid();
            template0.IsRequired = true;

            var template1 = model.MemberProfileItemTemplates[1];
            template1.ItemName = ShortGuid.NewGuid();
            template1.IsRequired = false;

            model.MemberProfileItemTemplates[2].ItemName = "  ";

            model.Id = await _vs.PostAsync(appUserId, model);

            return model;
        }
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
        public async Task Create_test_community_from_repository()
        {
            var communityTestDataProvider = _fixture.GetService<ICommunityTestDataProvider>();
            var community = await communityTestDataProvider.CreateTestCommunityFromRepository();

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
