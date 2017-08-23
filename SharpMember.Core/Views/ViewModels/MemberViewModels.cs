using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberIndexItemVM 
    {
        public bool Selected { get; set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int MemberNumber { get; internal set; }
        public bool Renewed { get; internal set; }
    }

    public class MemberIndexVM
    {
        public List<MemberIndexItemVM> ItemViewModels { get; set; } = new List<MemberIndexItemVM>();
    }

    public class MemberUpdateVM : MemberEntity
    {
        public List<MemberProfileItemEntity> MemberProfileItems { get; set; } = new List<MemberProfileItemEntity>();

        public MemberUpdateVM() { }

        public MemberUpdateVM(Member member)
        {
            this.CancellationDate = member.CancellationDate;
            this.Id = member.Id;
            this.Level = member.Level;
            this.MemberNumber = member.MemberNumber;
            this.Name = member.Name;
            this.OrganizationRole = member.OrganizationRole;
            this.RegistrationDate = member.RegistrationDate;
            this.Remarks = member.Remarks;
            this.Renewed = member.Renewed;

            this.MemberProfileItems = member.MemberProfileItems.Select(p => new MemberProfileItemEntity(p)).ToList();
        }

        Member
    }
}
