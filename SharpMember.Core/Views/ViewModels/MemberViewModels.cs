using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberProfileItemVM
    {
        public int Id { get; set; }
        public string ItemValue { get; set; }
        public string ItemName { get; set; }
        public int MemberProfileItemTemplateId { get; set; }

        public MemberProfileItemVM() { }

        public MemberProfileItemVM(MemberProfileItem item, string itemName)
        {
            this.Id = item.Id;
            this.ItemValue = item.ItemValue;
            this.MemberProfileItemTemplateId = item.MemberProfileItemTemplateId;

            this.ItemName = itemName;
        }
    }

    public class MemberUpdateVM : MemberEntity
    {
        public List<MemberProfileItemVM> ProfileItemViewModels { get; set; } = new List<MemberProfileItemVM>();
    }
}
