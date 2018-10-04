using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberProfileItemTemplateVm
    {
        public MemberProfileItemTemplate ItemTemplate { get; set; } = new MemberProfileItemTemplate();
        public bool Delete { get; set; } = false;
    }

    public class CommunityUpdateVm : CommunityEntity
    {
        public List<MemberProfileItemTemplateVm> ItemTemplateVMs { get; set; } = new List<MemberProfileItemTemplateVm>();

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
