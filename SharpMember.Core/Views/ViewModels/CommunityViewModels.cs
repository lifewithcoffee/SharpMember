using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class CommunityIndexItemVM : CommunityEntity
    {
        public bool Selected { get; set; } = false;
    }

    public class CommunityIndexVM
    {
        public List<CommunityIndexItemVM> ItemViewModels { get; set; } = new List<CommunityIndexItemVM>();
    }

    public class CommunityMemberItemVM
    {
        public bool Selected { get; set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int MemberNumber { get; internal set; }
        public bool Renewed { get; internal set; }
    }

    public class CommunityMembersVM
    {
        public int CommunityId { get; set; }
        public List<CommunityMemberItemVM> ItemViewModels { get; set; } = new List<CommunityMemberItemVM>();
    }
    
    public class CommunityUpdateVM : CommunityEntity
    {
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();
    }
}
