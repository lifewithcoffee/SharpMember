using SharpMember.Core.Data.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels.CommunityVms
{
    public class MemberProfileItemTemplateVm
    {
        public MemberProfileItemTemplate ItemTemplate { get; set; } = new MemberProfileItemTemplate();
        public bool Delete { get; set; } = false;
    }

    public class CommunityUpdateVm : CommunityEntity
    {
        public List<MemberProfileItemTemplateVm> ItemTemplateVMs { get; set; } = new List<MemberProfileItemTemplateVm>();
    }
}
