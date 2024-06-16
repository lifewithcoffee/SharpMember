using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.Community
{
    public class CommunityEntity
    {
        public int Id { get; set; } // will be actually used as a tenant id
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Announcement { get; set; }
    }

    public static class CommunityEntityExt
    {
        public static TOut CopyFrom<TOut>(this TOut to, CommunityEntity from)
            where TOut  : CommunityEntity
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.Introduction = from.Introduction;
            to.Announcement = from.Announcement;

            return to;
        }
    }

    public class Community : CommunityEntity
    {
        public virtual List<Member> Members { get; set; } = new List<Member>();
        public virtual List<Group> Groups { get; set; } = new List<Group>();
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();

        public CommunityUpdateVm ConvertToCommunityUpdateVM()
        {
            var result = new CommunityUpdateVm().CopyFrom(this);
            result.ItemTemplateVMs = this.MemberProfileItemTemplates
                                         .Select(x => new MemberProfileItemTemplateVm { ItemTemplate = x})
                                         .ToList();
            return result;
        }
    }
}
