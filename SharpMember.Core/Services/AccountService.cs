using NetCoreUtils.Database;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.DataServices;
using SharpMember.Core.Data.DataServices.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Services
{
    public interface IAccountService
    {
        Task AttachProfileToUserAsync(int userId, int profileId);
    }

    public class AccountService : IAccountService
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<ApplicationUser> _userRepository;

        public AccountService(IRepository<Member> memberRepository, IRepository<ApplicationUser> userRepository)
        {
            this._memberRepository = memberRepository;
            this._userRepository = userRepository;
        }

        public async Task AttachProfileToUserAsync(int userId, int profileId)
        {
            var user = await _userRepository.GetAsync(userId);
            var profile = await _memberRepository.GetAsync(profileId);

            user.Members.Add(profile);
        }
    }
}
