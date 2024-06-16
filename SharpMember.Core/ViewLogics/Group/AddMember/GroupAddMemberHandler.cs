﻿using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharpMember.Core.Data.Models.Community;
using System.Threading.Tasks;
using NetCoreUtils.Database;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupAddMemberHandler
    {
        GroupAddMemberVm Get(int groupId);
        Task PostAsync(GroupAddMemberVm vm);
    }

    public class GroupAddMemberHandler : IGroupAddMemberHandler
    {
        IRepository<Group> _groupRepo;
        IRepository<Member> _memberRepo;
        IRepository<GroupMemberRelation> _groupMemberRelationRepo;

        public GroupAddMemberHandler(IRepository<Member> memberRepo, IRepository<GroupMemberRelation> groupMemberRelationRepo, IRepository<Group> groupRepo)
        {
            _memberRepo = memberRepo;
            _groupMemberRelationRepo = groupMemberRelationRepo;
            _groupRepo = groupRepo;
        }

        public GroupAddMemberVm Get(int groupId)
        {
            var communityId = _groupRepo.Get(groupId).CommunityId;

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

            var allCommunityMemberQuery = _memberRepo.Query(x => x.CommunityId == communityId);
            var groupMemberRelationQuery = _groupMemberRelationRepo.QueryAll();
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
