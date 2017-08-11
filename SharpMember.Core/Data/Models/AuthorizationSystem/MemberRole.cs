using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpMember.Core.Data.Models.AuthorizationSystem
{
    public class MemberRoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int OrganizationId { get; set; }
    }

    public class MemberRole : MemberRoleEntity
    {
        public virtual List<Member> Members { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization;

        public virtual List<Permission> Permissions { get; set; }
    }
}
