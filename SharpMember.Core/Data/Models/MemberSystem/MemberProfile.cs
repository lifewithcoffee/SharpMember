using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;

public class MemberProfileEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class MemberProfile : MemberProfileEntity
{
    public virtual List<Member> Members { get; set; } = new List<Member>();
    public virtual List<MemberProfileItem> MemberProfileItems { get; set; } = new List<MemberProfileItem>();
}