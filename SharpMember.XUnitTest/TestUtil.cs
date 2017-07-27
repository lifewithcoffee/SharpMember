using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace U
{
    class TestUtil : DependencyEnabled
    {
        public int GetExistingOrganizationId()
        {
            var repo = this.serviceProvider.GetService<IOrganizationRepository>();
            var org = repo.Add(Guid.NewGuid().ToString());
            repo.Commit();
            return org.Id;
        }

        public int GetNonexistentOrganizationId()
        {
            var repo = this.serviceProvider.GetService<IOrganizationRepository>();
            var org = repo.GetAll().OrderBy(o => o.Id).LastOrDefault();
            if (org == null)
            {
                return new Random().Next(); // return non-negative integer
            }
            else
            {
                return org.Id + new Random().Next();
            }

        }
    }
}
