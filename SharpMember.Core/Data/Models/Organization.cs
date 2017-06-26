using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models
{
    public class OrganizationEntity
    {
        public int Id { get; set; } // will be actually used as a tenant id
        public string Name { get; set; }
    }

    public class Organization : OrganizationEntity
    {
        public virtual List<Branch> Branches { get; set; } = new List<Branch>();
    }
}
