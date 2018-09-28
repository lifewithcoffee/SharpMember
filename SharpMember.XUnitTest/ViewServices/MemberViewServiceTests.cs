using System;
using System.Collections.Generic;
using System.Text;
using U.TestEnv;
using Xunit;

namespace U.ViewServices
{
    [Collection(nameof(ServiceProviderCollection))]
    public class MemberViewServiceTests
    {
        ServiceProviderFixture _fixture;

        public MemberViewServiceTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Member_profile_should_be_consist_with_member_profile_template()
        {

        }
    }
}
