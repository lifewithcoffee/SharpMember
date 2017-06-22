using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Data.Models
{
    public class BranchEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Branch : BranchEntity
    {
        public Organization Organization { get; set; }
        public virtual List<Member2> Members { get; set; } = new List<Member2>();
    }
}
