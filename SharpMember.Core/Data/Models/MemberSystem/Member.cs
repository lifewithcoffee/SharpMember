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
        public string Name { get; set; }
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int Level { get; set; }
        public string Remarks { get; set; }

        public int OrganizationId { get; set; }
        public int ApplicationUserId { get; set; }
    }

    public class Member : MemberEntity
    {
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        public virtual List<MemberProfileItem> MemberProfileItems { get; set; } = new List<MemberProfileItem>();
        public virtual List<MemberMemberGroupRelation> MemberMemberGroupRelations { get; set; } = new List<MemberMemberGroupRelation>();
        //[InverseProperty(nameof(WorkTask.WorkTaskOwner))]
        //public virtual List<WorkTask> OwnedWorkTasks { get; set; } = new List<WorkTask>();

        //[InverseProperty(nameof(WorkTask.WorkTaskCreator))]
        //public virtual List<WorkTask> CreatedWorkTasks { get; set; } = new List<WorkTask>();
    }
}