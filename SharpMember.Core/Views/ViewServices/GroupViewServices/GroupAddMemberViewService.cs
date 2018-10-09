using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupAddMemberViewService
    {
        GroupAddMemberVm Get(int groupId);
        Task PostAsync(GroupAddMemberVm vm);
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

        public GroupAddMemberVm Get(int groupId)
        {
            var communityId = _groupRepo.GetById(groupId).CommunityId;

            //var items = (from m in _memberRepo.GetMany(x => x.CommunityId == communityId) select m).ToList()
            //            .Except(
            //                from member in _memberRepo.GetMany(x => x.CommunityId == communityId)
            //                join relation in _groupMemberRelationRepo.GetAll() on member.Id equals relation.MemberId
            //                where relation.GroupId == groupId select member)
            //            .Select(member => new MemberItemVm { Id = member.Id
            //                                                , Name = string.IsNullOrWhiteSpace(member.Name) ? "(No name)" : member.Name, MemberNumber = member.MemberNumber
            //                                                , Renewed = member.Renewed
            //                                                })
            //            .ToList();

            var allCommunityMemberQuery = _memberRepo.GetMany(x => x.CommunityId == communityId);
            var groupMemberRelationQuery = _groupMemberRelationRepo.GetAll();
            var items = (from m in allCommunityMemberQuery
                         where !(from m2 in allCommunityMemberQuery
                                 join r in groupMemberRelationQuery on m2.Id equals r.MemberId
                                 where r.GroupId == groupId select m2.Id).Contains(m.Id)
                         select new MemberItemVm { Id = m.Id
                                                 , Name = string.IsNullOrWhiteSpace(m.Name) ? "(no name)" : m.Name
                                                 , MemberNumber = m.MemberNumber
                                                 , Renewed = m.Renewed }
                         ).ToList();

            return new GroupAddMemberVm { GroupId = groupId, MemberItemVms = items };
        }

        public async Task PostAsync(GroupAddMemberVm vm)
        {
            _groupMemberRelationRepo.AddRange(vm.MemberItemVms.Where(x => x.Selected).Select(x => new GroupMemberRelation { GroupId = vm.GroupId, MemberId = x.Id }));
            await _groupMemberRelationRepo.CommitAsync();
        }
    }
}
