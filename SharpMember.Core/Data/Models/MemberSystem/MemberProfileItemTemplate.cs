
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;

public class MemberProfileItemTemplateEntity
{
    public int Id { get; set; }    
    public string ItemName { get; set; }
    public bool IsRequired { get; set; } = false;
    public int OrganizationId { get; set; }
}

public class MemberProfileItemTemplate : MemberProfileItemTemplateEntity
{
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
}
