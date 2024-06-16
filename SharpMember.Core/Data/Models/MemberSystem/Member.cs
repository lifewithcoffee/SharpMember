using SharpMember.Core.Data.Models.ActivitySystem;
using SharpMember.Core.Data.Models.ProjectSystem;
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
        public string CommunityRole { get; set; }
        public string Email { get; set; }   // for auto-connecting a member to an ApplicationUser
        public int MemberNumber { get; set; } = 0;
        public bool Renewed { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int Level { get; set; }
        public string Remarks { get; set; }

        // FK
        public int CommunityId { get; set; }
        public string ApplicationUserId { get; set; }
    }

    public static class MemberEntityExt
    {
        public static TOut CopyFrom<TOut>(this TOut to, MemberEntity from)
            where TOut : MemberEntity
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.CommunityRole = from.CommunityRole;
            to.Email = from.Email;
            to.MemberNumber = from.MemberNumber;
            to.Renewed = from.Renewed;
            to.RegistrationDate = from.RegistrationDate;
            to.CancellationDate = from.CancellationDate;
            to.Level = from.Level;
            to.Remarks = from.Remarks;

            to.CommunityId = from.CommunityId;
            to.ApplicationUserId = from.ApplicationUserId;

            return to;
        }
    }

    public class Member : MemberEntity
    {
        [ForeignKey(nameof(CommunityId))]
        public virtual Community Community { get; set; }

        public virtual List<MemberProfileItem> MemberProfileItems { get; set; } = new List<MemberProfileItem>();
        public virtual List<GroupMemberRelation> GroupMemberRelations { get; set; } = new List<GroupMemberRelation>();

        //[InverseProperty(nameof(WorkTask.WorkTaskOwner))]
        //public virtual List<WorkTask> OwnedWorkTasks { get; set; } = new List<WorkTask>();

        //[InverseProperty(nameof(WorkTask.WorkTaskCreator))]
        //public virtual List<WorkTask> CreatedWorkTasks { get; set; } = new List<WorkTask>();
    }
}