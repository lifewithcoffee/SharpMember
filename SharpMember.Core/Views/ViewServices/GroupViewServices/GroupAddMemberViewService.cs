using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupAddMemberViewService
    {
        GroupAddMemberVm GetAsync(int groupId);
        void Post(GroupAddMemberVm vm);
    }

    public class GroupAddMemberViewService : IGroupAddMemberViewService
    {
        IGroupRepository _groupRepo;
        IMemberRepository _memberRepo;
        IGroupMemberRelationRepository _groupMemberRelationRepo;

        public GroupAddMemberViewService(IMemberRepository memberRepo, IGroupMemberRelationRepository groupMemberRelationRepo, IGroupRepository groupRepo)
        {
            _memberRepo = memberRepo;
            _groupMemberRelationRepo = groupMemberRelationRepo;
            _groupRepo = groupRepo;
        }

        public GroupAddMemberVm GetAsync(int groupId)
        {
            var communityId = _groupRepo.GetById(groupId).CommunityId;

            var items = (from member in _memberRepo.GetAll()
                         join relation in _groupMemberRelationRepo.GetAll() on member.Id equals relation.MemberId into relations
                         from r in relations.DefaultIfEmpty()
                         where r.GroupId != groupId && member.CommunityId == communityId
                         select new MemberItemVm { Id = member.Id
                                                 , Name = string.IsNullOrWhiteSpace(member.Name) ? "(No name)" : member.Name, MemberNumber = member.MemberNumber
                                                 , Renewed = member.Renewed
                                                 }
                         ).ToList();
                        
            return new GroupAddMemberVm { GroupId = groupId, MemberItemVms = items };
        }

        public void Post(GroupAddMemberVm vm)
        {
            throw new NotImplementedException();
        }
    }
}
