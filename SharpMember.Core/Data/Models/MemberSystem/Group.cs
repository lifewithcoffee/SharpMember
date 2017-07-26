﻿using System;
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
        public string Introduction { get; set; }
        public string Announcement { get; set; }
    }

    /// <summary>
    /// The relationship between Member and MemberGroup is many-to-many.
    /// So MemberGroup is actuall a label system for members.
    /// </summary>
    public class Group : GroupEntity
    {
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        public virtual List<GroupMemberRelation> GroupMemberRelations { get; set; } = new List<GroupMemberRelation>();
    }
}