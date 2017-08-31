using AutoMapper;
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
            return Mapper.Map<TSource, TTarget>(source);
        }
    }
}
