using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DataServiceNew
{
    interface IMemberServiceNew
    {
        Task<int> AssignMemberNubmerAsync(int memberId, int nextMemberNumber);
        Member Add(Member entity);
    }

    //class Member
    //{
    //}
}
