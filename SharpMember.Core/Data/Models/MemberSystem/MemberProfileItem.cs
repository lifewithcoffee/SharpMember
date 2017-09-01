using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberProfileItemEntity
    {
        public int Id { get; set; }
        public string ItemValue { get; set; }
    }

    public class MemberProfileItemWithFK : MemberProfileItemEntity
    { 
        // FK
        public int MemberId { get; set; }
        public int MemberProfileItemTemplateId { get; set; }
    }

    public class MemberProfileItem : MemberProfileItemWithFK
    {
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }

        [ForeignKey(nameof(MemberProfileItemTemplateId))]
        public virtual MemberProfileItemTemplate MemberProfileItemTemplate { get; set; }

        public MemberProfileItem() { }

        public MemberProfileItem(MemberProfileItemVM viewModel)
        {
            this.Id = viewModel.Id;
            this.ItemValue = viewModel.ItemValue;
            this.MemberProfileItemTemplateId = viewModel.MemberProfileItemTemplateId;
        }
    }
}
