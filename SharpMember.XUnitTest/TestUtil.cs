﻿using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SharpMember.Core.Data.Models.MemberSystem;

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
            if (null == org)
            {
                return 1;
            }
            else
            {
                return org.Id + 1;
            }
        }

        public int GetExistingMemberId()
        {
            int existingOrgId = this.GetExistingOrganizationId();
            var repo = this.serviceProvider.GetService<IMemberRepository>();
            var member = repo.Add(new Member { OrganizationId = existingOrgId });
            repo.Commit();
            return member.Id;
        }

        public int GetNonexistentMemberId()
        {
            var memberRepo = this.serviceProvider.GetService<IMemberRepository>();
            var member = memberRepo.GetAll().OrderBy(m => m.Id).LastOrDefault();
            if(null == member)
            {
                return 1;
            }
            else
            {
                return member.Id + 1;
            }
        }
    }
}