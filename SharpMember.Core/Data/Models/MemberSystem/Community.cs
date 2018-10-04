using AutoMapper;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class CommunityEntity
    {
        public int Id { get; set; } // will be actually used as a tenant id
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Announcement { get; set; }
    }

    public class Community : CommunityEntity
    {
        public virtual List<Member> Members { get; set; } = new List<Member>();
        public virtual List<Group> Groups { get; set; } = new List<Group>();
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();

        public CommunityUpdateVm ConvertToCommunityUpdateVM()
        {
            CommunityUpdateVm result = new CommunityUpdateVm();
            result.Id = this.Id;
            result.Name = this.Name;
            result.Introduction = this.Introduction;
            result.Announcement = this.Announcement;
            result.ItemTemplateVMs = this.MemberProfileItemTemplates.Select(x => new MemberProfileItemTemplateVm { ItemTemplate = x}).ToList();
            return result;
        }
    }
}
