using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpMember.Core.Data.Models.AuthorizationSystem
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Operation { get; set; }

        public int MemberRoleId { get; set; }
    }

    public class Permission : PermissionEntity
    {
        [ForeignKey(nameof(MemberRoleId))]
        public virtual MemberRole MemberRole { get; set; }
    }
}
