using SharpMember.Core.Data.Models.ActivitySystem;
using SharpMember.Core.Data.Models.TaskSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberEntity
    {
        public int Id { get; set; } // some members may not have been assigned a member number, so an Id field is still required
        public int MemberNumber { get; set; }
        public string Name { get; set; }
        public bool Renewed { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public int Level { get; set; }
        public string Remarks { get; set; }
    }

    public class Member : MemberEntity
    {
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(MemberProfileId))]
        public virtual MemberProfile MemberProfile { get; set; }
        public int MemberProfileId { get; set; }

        public virtual List<MemberMemberGroupRelation> MemberGroupRelations { get; set; } = new List<MemberMemberGroupRelation>();
        //[InverseProperty(nameof(WorkTask.WorkTaskOwner))]
        //public virtual List<WorkTask> OwnedWorkTasks { get; set; } = new List<WorkTask>();

        //[InverseProperty(nameof(WorkTask.WorkTaskCreator))]
        //public virtual List<WorkTask> CreatedWorkTasks { get; set; } = new List<WorkTask>();
    }
}