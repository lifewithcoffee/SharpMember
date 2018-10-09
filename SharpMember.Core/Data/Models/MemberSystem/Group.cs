using AutoMapper;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CommunityId { get; set; }
    }

    /// <summary>
    /// The relationship between Member and MemberGroup is many-to-many.
    /// So MemberGroup is actuall a label system for members.
    /// </summary>
    public class Group : GroupEntity
    {
        [ForeignKey(nameof(CommunityId))]
        public virtual Community Community { get; set; }

        public virtual List<GroupMemberRelation> GroupMemberRelations { get; set; } = new List<GroupMemberRelation>();

        public GroupUpdateVm ConvertToGroupUpdateVM()
        {
            return Mapper.Map<Group, GroupUpdateVm>(this);
        }
    }
}
