using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Mappers
{
    public static class MemberProfileItemMapper<TSource, TTarget>
        where TSource : MemberProfileItemEntity
        where TTarget : MemberProfileItemEntity, new()
    {
        public static TTarget Cast(TSource source)
        {
            TTarget result = new TTarget();

            result.Id = source.Id;
            result.IsRequired = source.IsRequired;
            result.ItemName = source.ItemName;
            result.ItemValue = source.ItemValue;

            return result;
        }
    }
}
