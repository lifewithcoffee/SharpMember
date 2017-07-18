using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberIndexViewItemModel { }

    public class MemberIndexViewModel { }

    public class MemberCreateViewModel : MemberEntity
    {
        public virtual List<MemberProfileItem> MemberProfileItems { get; set; } = new List<MemberProfileItem>();
    }
}
