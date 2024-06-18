﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetCoreUtils.Database;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Data.DataServices.MemberSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class GroupRoleRequirement : IAuthorizationRequirement
    {
        public string MemberRole { get; private set; }

        public GroupRoleRequirement(string memberRole)
        {
            this.MemberRole = memberRole;
        }
    }

    public class GroupRoleHandler : AuthorizationHandler<GroupRoleRequirement,Group>
    {
        UserManager<ApplicationUser> _userManager;
        IRepository<Member> _memberRepo;
        IRepository<GroupMemberRelation> _memberGroupRoleRelationRepo;

        public GroupRoleHandler(UserManager<ApplicationUser> userManager, IRepository<Member> memberRepo, IRepository<GroupMemberRelation> memberGroupRoleRelationRepo)
        {
            this._userManager = userManager;
            this._memberRepo = memberRepo;
            this._memberGroupRoleRelationRepo = memberGroupRoleRelationRepo;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRoleRequirement requirement, Group group)
        {
            //var user = await _userManager.GetUserAsync(context.User);
            //string userId = user.Id;

            string userId = _userManager.GetUserId(context.User);

            Member member = this._memberRepo.Query(m => m.ApplicationUserId == userId).SingleOrDefault();
            if (!string.IsNullOrWhiteSpace(member?.CommunityRole))
            {
                context.Succeed(requirement);
            }

            string groupRole = _memberGroupRoleRelationRepo.Query(m => m.GroupId == group.Id && m.MemberId == member.Id).SingleOrDefault()?.GroupRole;
            if(groupRole == requirement.MemberRole)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
