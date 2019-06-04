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
using NetCoreUtils.Diagnosis;
using SharpMember.Core.Views.ViewModels.CommunityVms;

namespace U.TestEnv.TestService
{
    interface ICommunityTestDataProvider
    {
        Task<Community> CreateTestCommunityFromRepository();
        Task<(string appUserId, CommunityUpdateVm)> CreateTestCommunityFromViewService();
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
        /// 
        /// Added member number: 10
        /// Added member profile template item number: 8 (5 required, 3 optional)
        /// </summary>
        public async Task<Community> CreateTestCommunityFromRepository()
        {
            var _communityService = _serviceProvider.GetService<ICommunityService>();
            var _groupMemberRelationRepo = _serviceProvider.GetService<IRepository<GroupMemberRelation>> ();

            await _communityService.CreateCommunityAsync(await util.GetExistingAppUserId(), $"TestCommunityFromRepo_{ShortGuid.NewGuid()}");

            await AddGroupedMembers(_communityService, _groupMemberRelationRepo, 2);
            await AddGroupedMembers(_communityService, _groupMemberRelationRepo, 3);
            await AddGroupedMembers(_communityService, _groupMemberRelationRepo, 4);
            await _communityService.AddMemberAsync(null, ShortGuid.NewGuid(), "", "");

            // add member profile templates (5 required, 3 optional)
            for (int i = 0; i < 5; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), true);

            for (int i = 0; i < 3; i++)
                await _communityService.AddMemberProfileTemplateAsync(ShortGuid.NewGuid(), false);

            await _communityService.CommitAsync();
            return _communityService.Community;

            async Task AddGroupedMembers(ICommunityService communityService, IRepository<GroupMemberRelation> groupMemberRelationRepo ,int memberNumber)
            {
                Group group = new Group { Name = ShortGuid.NewGuid(), Community = communityService.Community };

                for (int i = 0; i < memberNumber; i++)
                {
                    var member = await communityService.AddMemberAsync(null, ShortGuid.NewGuid(), "", "");

                    GroupMemberRelation relation = new GroupMemberRelation { Group = group, Member = member };
                    groupMemberRelationRepo.Add(relation);
                }
            }
        }

        /// <summary>
        /// Added member profile template number: 2 (one required, one optional)
        /// </summary>
        public async Task<(string appUserId,CommunityUpdateVm)> CreateTestCommunityFromViewService()
        {
            var _vs = _serviceProvider.GetService<ICommunityCreateHandler>();

            string appUserId = await util.GetExistingAppUserId();

            CommunityUpdateVm model = _vs.Get();
            model.Name = ShortGuid.NewGuid();

            var template0 = model.ItemTemplateVMs[0];
            template0.ItemTemplate.ItemName = ShortGuid.NewGuid();
            template0.ItemTemplate.IsRequired = true;

            var template1 = model.ItemTemplateVMs[1];
            template1.ItemTemplate.ItemName = ShortGuid.NewGuid();
            template1.ItemTemplate.IsRequired = false;

            model.ItemTemplateVMs[2].ItemTemplate.ItemName = "  ";

            model.Id = await _vs.PostAsync(appUserId, model);

            Ensure.IsTrue(!string.IsNullOrWhiteSpace(appUserId));
            Ensure.IsTrue(model.Id > 0);

            return (appUserId, model);
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
                                .Repo
                                .Query(c => c.Id == community.Id)
                                .Include(c => c.Groups)
                                    .ThenInclude(g => g.GroupMemberRelations)
                                .Include(c => c.Members)
                                .Include(c => c.MemberProfileItemTemplates)
                                .Single();

            Assert.True(community.Id > 0);
            Assert.Equal(11, community.Members.Count);    // 1 owner self's and 10 new added
            Assert.Equal(8, community.MemberProfileItemTemplates.Count);
            Assert.Equal(3, community.Groups.Count);

            Assert.Equal(2, community.Groups[0].GroupMemberRelations.Count);
            Assert.Equal(3, community.Groups[1].GroupMemberRelations.Count);
            Assert.Equal(4, community.Groups[2].GroupMemberRelations.Count);
        }
    }
}
