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
    
    public class CommunityUpdateVM : CommunityEntity
    {
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();
    }
}
