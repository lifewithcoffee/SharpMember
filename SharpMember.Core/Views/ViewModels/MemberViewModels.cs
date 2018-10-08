using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberProfileItemVm
    {
        public int Id { get; set; }
        public string ItemValue { get; set; }
        public string ItemName { get; set; }
        public int MemberProfileItemTemplateId { get; set; }

        public MemberProfileItemVm() { }

        public MemberProfileItemVm(MemberProfileItem item, string itemName)
        {
            this.Id = item.Id;
            this.ItemValue = item.ItemValue;
            this.MemberProfileItemTemplateId = item.MemberProfileItemTemplateId;

            this.ItemName = itemName;
        }
    }

    public class MemberUpdateVm : MemberEntity
    {
        public List<MemberProfileItemVm> ProfileItemViewModels { get; set; } = new List<MemberProfileItemVm>();
    }
}
