﻿using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityMembersHandler
    {
        CommunityMembersVm Get(int commId);
        Task PostToDeleteSelected(CommunityMembersVm data);
    }

    public class CommunityMembersHandler : ICommunityMembersHandler
    {
        IRepository<Member> _memberRepo;

        public CommunityMembersHandler(IRepository<Member> memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public CommunityMembersVm Get(int commId)
        {
            var items = _memberRepo
                .Query(m => m.CommunityId == commId)
                .Select(m => new MemberItemVm { Id = m.Id, Name = string.IsNullOrWhiteSpace(m.Name) ? "(No name)" : m.Name, MemberNumber = m.MemberNumber, Renewed = m.Renewed })
                .ToList();

            return new CommunityMembersVm {  CommunityId = commId, MemberItemVms = items };
        }

        public async Task PostToDeleteSelected(CommunityMembersVm data)
        {
            _memberRepo.RemoveRange(data.MemberItemVms.Where(x => x.Selected).Select(x => new Member { Id = x.Id }));
            await _memberRepo.CommitAsync();
        }
    }
}
