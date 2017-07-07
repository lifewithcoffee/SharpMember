using SharpMember.Core.Data.Repositories;
using SharpMember.Core.Data.Repositories.MemberSystem;
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
        private readonly IMemberRepository _memberRepository;
        private readonly IApplicationUserRepository _userRepository;

        public AccountService(IMemberRepository memberRepository, IApplicationUserRepository userRepository)
        {
            this._memberRepository = memberRepository;
            this._userRepository = userRepository;
        }

        public async Task AttachProfileToUserAsync(int userId, int profileId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var profile = await _memberRepository.GetByIdAsync(profileId);

            user.Members.Add(profile);
        }
    }
}
