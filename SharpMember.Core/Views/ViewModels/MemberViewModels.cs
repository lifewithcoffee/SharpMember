using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberIndexItemViewModel : MemberEntity
    {
        public bool Selected { get; set; }
    }

    public class MemberIndexViewModel
    {
        public List<MemberIndexItemViewModel> ItemViewModels { get; set; } = new List<MemberIndexItemViewModel>();
    }

    public class MemberCreateViewModel : MemberEntity
    {
        public List<MemberProfileItemEntity> MemberProfileItems { get; set; } = new List<MemberProfileItemEntity>();
    }
}
