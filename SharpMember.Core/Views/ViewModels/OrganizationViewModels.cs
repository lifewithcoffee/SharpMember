using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class OrganizationIndexItemVM : OrganizationEntity
    {
        public bool Selected { get; set; }
    }

    public class OrganizationIndexVM
    {
        public List<OrganizationIndexItemVM> ItemViewModels { get; set; } = new List<OrganizationIndexItemVM>();
    }
    
    public class OrganizationCreateVM : OrganizationEntity
    {
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();
    }
}
