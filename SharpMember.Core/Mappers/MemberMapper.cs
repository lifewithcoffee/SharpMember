using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Mappers
{
    public static class MemberMapper<TSource, TTarget>
        where TSource : MemberEntity
        where TTarget : MemberEntity, new()
    {
        public static TTarget Cast(TSource source)
        {
            TTarget result = new TTarget();

            result.CancellationDate = source.CancellationDate;
            result.Id = source.Id;
            result.Level = source.Level;
            result.MemberNumber = source.MemberNumber;
            result.Name = source.Name;
            result.OrganizationRole = source.OrganizationRole;
            result.RegistrationDate = source.RegistrationDate;
            result.Remarks = source.Remarks;
            result.Renewed = source.Renewed;

            return result;
        }
    }
}
