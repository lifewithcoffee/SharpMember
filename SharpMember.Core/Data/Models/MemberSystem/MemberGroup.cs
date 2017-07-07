using System;
using System.Collections.Generic;
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
        public Organization Organization { get; set; }
        public virtual List<MemberGroupRelation> MemberGroupRelations { get; set; } = new List<MemberGroupRelation>();
        public IEnumerable<object> Members { get; internal set; }
    }
}
