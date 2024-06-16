using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.Community
{
    public class MemberProfileItemEntity
    {
        public int Id { get; set; }
        public string ItemValue { get; set; }

        // FKs
        public int MemberId { get; set; }
        public int MemberProfileItemTemplateId { get; set; }
    }

    public static class MemberProfileItemEntityExt
    {

        public static TOut CopyFrom<TOut>(this TOut to, MemberProfileItemEntity from)
            where TOut  : MemberProfileItemEntity
        {
            to.Id = from.Id;
            to.ItemValue = from.ItemValue;
            to.MemberId = from.MemberId;
            to.MemberProfileItemTemplateId = from.MemberProfileItemTemplateId;

            return to;
        }
    }

    public class MemberProfileItem : MemberProfileItemEntity
    {
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }

        [ForeignKey(nameof(MemberProfileItemTemplateId))]
        public virtual MemberProfileItemTemplate MemberProfileItemTemplate { get; set; }

        public MemberProfileItem() { }

        public MemberProfileItem(MemberProfileItemVm viewModel)
        {
            this.Id = viewModel.Id;
            this.ItemValue = viewModel.ItemValue;
            this.MemberProfileItemTemplateId = viewModel.MemberProfileItemTemplateId;
        }
    }
}
