using SharpMember.Core.Data.DataServices.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SharpMember.Core.Data.Models.Community;
using NetCoreUtils.Database;

namespace SharpMember.Core.Services
{
    public interface IAuthorizationService
    {
        void UpdateCommunityRole(int memberId, string roleName);
        void RemoveCommunityRole(int memberId);
        void UpdateGroupRole(int memberId, int groupId, string roleName);
        void RemoveGroupRole(int memberId, int groupId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        IRepository<Member> _memberRepo;
        IRepository<GroupMemberRelation> _memberGroupRoleRelationRepository;

        public AuthorizationService(IRepository<Member> memberRepo, IRepository<GroupMemberRelation> memberGroupRoleRelationRepository)
        {
            this._memberRepo = memberRepo;
            this._memberGroupRoleRelationRepository = memberGroupRoleRelationRepository;
        }

        public void UpdateCommunityRole(int memberId, string roleName)
        {
            _memberRepo.Get(memberId).CommunityRole = roleName;
        }

        public void RemoveCommunityRole(int memberId)
        {
            _memberRepo.Get(memberId).CommunityRole = "";
        }

        public void UpdateGroupRole(int memberId, int groupId, string roleName)
        {
            var relation = _memberGroupRoleRelationRepository.Query(m => m.MemberId == memberId && m.GroupId == groupId).SingleOrDefault();

            if(relation != null)
            {
                relation.GroupRole = roleName;
            }
            else
            {
                _memberGroupRoleRelationRepository.Add(new GroupMemberRelation { MemberId = memberId, GroupId = groupId, GroupRole = roleName });
            }
        }

        public void RemoveGroupRole(int memberId, int groupId)
        {
            _memberGroupRoleRelationRepository.Remove(m => m.MemberId == memberId && m.GroupId == groupId);
        }
    }
}
