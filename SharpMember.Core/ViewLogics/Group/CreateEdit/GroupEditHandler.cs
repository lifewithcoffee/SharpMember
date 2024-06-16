﻿using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Utils.Mappers;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupEditHandler
    {
        GroupUpdateVm Get(int id);
        Task PostToUpdateAsync(GroupUpdateVm data);
        Task PostToDeleteSelectedMembersAsync(GroupUpdateVm data);
    }

    public class GroupEditHandler : IGroupEditHandler
    {
        IRepository<Group> _groupRepository;

        public GroupEditHandler(IRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupUpdateVm Get(int id)
        {
            var group = _groupRepository.Query(x => x.Id == id).Include(x => x.GroupMemberRelations).ThenInclude(x => x.Member).Single();
            var result = group.ConvertToGroupUpdateVM();
            result.MemberItemVms = group.GroupMemberRelations.Select(x =>
                new MemberItemVm
                {
                    Id = x.Member.Id,
                    MemberNumber = x.Member.MemberNumber,
                    Name = x.Member.Name,
                    Renewed = x.Member.Renewed,
                    Selected = false
                }).ToList();
            return result;
        }

        public async Task PostToUpdateAsync(GroupUpdateVm data)
        {
            Ensure.IsTrue(data.Id > 0);
            _groupRepository.Update(data.ConvertToGroup());
            await _groupRepository.CommitAsync();
        }

        public async Task PostToDeleteSelectedMembersAsync(GroupUpdateVm data)
        {
            Ensure.IsTrue(data.Id > 0);
            var group = _groupRepository.Query(x => x.Id == data.Id).Include(x => x.GroupMemberRelations).Single();
            foreach(var memberVm in data.MemberItemVms)
                group.GroupMemberRelations.RemoveAll(x => x.MemberId == memberVm.Id && memberVm.Selected);

            await _groupRepository.CommitAsync();
        }
    }
}
