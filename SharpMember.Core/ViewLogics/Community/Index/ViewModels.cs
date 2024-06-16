using SharpMember.Core.Data.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels.CommunityVms
{
    public class CommunityIndexItemVm : CommunityEntity
    {
        public bool Selected { get; set; } = false;
    }

    public class CommunityIndexVm
    {
        public List<CommunityIndexItemVm> ItemViewModels { get; set; } = new List<CommunityIndexItemVm>();
    }
}
