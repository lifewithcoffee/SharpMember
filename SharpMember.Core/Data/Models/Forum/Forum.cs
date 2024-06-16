using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.Forum;
public class ForumEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class Forum : ForumEntity
{
}
