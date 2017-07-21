using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberGroupEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// The relationship between Member and MemberGroup is many-to-many.
    /// So MemberGroup is actuall a label system for members.
    /// </summary>
    public class MemberGroup : MemberGroupEntity
    {
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        public virtual List<MemberMemberGroupRelation> MemberMemberGroupRelations { get; set; } = new List<MemberMemberGroupRelation>();
    }
}
