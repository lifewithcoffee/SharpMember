using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels.CommunityVms
{
    public class CommunityGroupItemVm
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
    }

    public class CommunityGroupsVm
    {
        public int CommunityId { get; set; }
        public List<CommunityGroupItemVm> ItemViewModels { get; set; } = new List<CommunityGroupItemVm>();
    }
}
