using SharpMember.Core.Data.Repositories;
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
        private readonly IMemberProfileRepository _memberProfileRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IMemberProfileRepository memberProfileRepository, IUserRepository userRepository)
        {
            this._memberProfileRepository = memberProfileRepository;
            this._userRepository = userRepository;
        }

        public async Task AttachProfileToUserAsync(int userId, int profileId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var profile = await _memberProfileRepository.GetByIdAsync(profileId);

            user.MemberProfiles.Add(profile);
        }
    }
}
