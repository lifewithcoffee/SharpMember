using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SharpMember.Core.Views.ViewModels.CommunityVms;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberProfileItemVm : MemberProfileItemEntity
    {
        public string ItemName { get; set; }
    }

    public class MemberUpdateVm : MemberEntity
    {
        public List<MemberProfileItemVm> ProfileItemViewModels { get; set; } = new List<MemberProfileItemVm>();
        public List<CommunityGroupItemVm> GroupList { get; set; } = new List<CommunityGroupItemVm>();
    }
}
