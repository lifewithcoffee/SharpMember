using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Utils.Mappers
{
    static public class ConvertTo
    {
        static public async Task<List<MemberProfileItemVm>> MemberProfileItemVMList(
            IList<MemberProfileItem> items,
            IRepository<MemberProfileItemTemplate> memberProfileItemTemplateRepository
        ){
            List<MemberProfileItemVm> result = new List<MemberProfileItemVm>();

            foreach (var item in items)
            {
                var template = await memberProfileItemTemplateRepository.GetAsync(item.MemberProfileItemTemplateId);
                string itemName = template.ItemName;

                var itemVm = new MemberProfileItemVm().CopyFrom(item);
                itemVm.ItemName = itemName;
                result.Add(itemVm);
            }

            return result;
        }

        static public async Task<List<MemberProfileItem>> MemberProfileItemList(
            IList<MemberProfileItemVm> items,
            IRepository<MemberProfileItemTemplate> memberProfileItemTemplateRepository
        ){
            List<MemberProfileItem> result = new List<MemberProfileItem>();

            foreach(var item in items)
            {
                var template = await memberProfileItemTemplateRepository.GetAsync(item.MemberProfileItemTemplateId);
                string itemName = template.ItemName;
                result.Add(new MemberProfileItem(item));
            }

            return result;
        }
    }
}
