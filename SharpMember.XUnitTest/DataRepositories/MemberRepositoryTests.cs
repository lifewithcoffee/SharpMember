using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace U.DataRepositories
{
    public class MemberRepositoryTests: DependencyEnabled
    {
        TestUtil util = new TestUtil();

        [Fact]
        public void TestOrganization_MemberProfileItemTemplate_change_should_cause_new_member_profile_item_change()
        {
            // create an organization and the relevant member item templates
            int existingOrgId = util.GetExistingOrganizationId();
            string[] originalTemplats = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            var itemTemplateRepo = this.serviceProvider.GetService<IMemberProfileItemTemplateRepository>();
            itemTemplateRepo.AddRquiredTemplatesAsync(existingOrgId, originalTemplats );
            itemTemplateRepo.Commit();

            // Generate a new member
            var memberRepo = this.serviceProvider.CreateScope().ServiceProvider.GetService<IMemberRepository>();
            var newMember = memberRepo.GenerateNewMember(existingOrgId);
        }
    }
}
