using AutoMapper;
using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool Selected { get; set; } = false;
        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
    }

    public class CommunityMembersVM
    {
        public int CommunityId { get; set; }
        public List<CommunityMemberItemVM> ItemViewModels { get; set; } = new List<CommunityMemberItemVM>();
    }

    public class CommunityGroupItemVM
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
    }

    public class CommunityGroupsVM
    {
        public int CommunityId { get; set; }
        public List<CommunityGroupItemVM> ItemViewModels { get; set; } = new List<CommunityGroupItemVM>();
    }
    
    public class MemberProfileItemTemplateVM
    {
        public MemberProfileItemTemplate ItemTemplate { get; set; } = new MemberProfileItemTemplate();
        public bool Delete { get; set; } = false;
    }

    public class CommunityUpdateVM : CommunityEntity
    {
        public List<MemberProfileItemTemplateVM> ItemTemplateVMs { get; set; } = new List<MemberProfileItemTemplateVM>();
        
        public Community ConvertToCommunityWithoutNavProp()
        {
            Community result = new Community();
            result.Id = this.Id;
            result.Name = this.Name;
            result.Introduction = this.Introduction;
            result.Announcement = this.Announcement;

            return result;
        }
    }
}
