using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Mappers
{
    static public class ConvertTo
    {
        static public async Task<List<MemberProfileItemVM>> MemberProfileItemVMList(
            IList<MemberProfileItem> items, 
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            List<MemberProfileItemVM> result = new List<MemberProfileItemVM>();

            foreach (var item in items)
            {
                var template = await memberProfileItemTemplateRepository.GetByIdAsync(item.MemberProfileItemTemplateId);
                string itemName = template.ItemName;
                result.Add(new MemberProfileItemVM(item, itemName));
            }

            return result;
        }

        static public async Task<List<MemberProfileItem>> MemberProfileItemList(
            IList<MemberProfileItemVM> items,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            List<MemberProfileItem> result = new List<MemberProfileItem>();

            foreach(var item in items)
            {
                var template = await memberProfileItemTemplateRepository.GetByIdAsync(item.MemberProfileItemTemplateId);
                string itemName = template.ItemName;
                result.Add(new MemberProfileItem(item));
            }

            return result;
        }
    }
}
