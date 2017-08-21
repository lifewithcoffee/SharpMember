using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices
{
    public class MemberIndexViewService : IViewService<MemberIndexVM>
    {
        public Task<MemberIndexVM> GetAsync()
        {
            MemberIndexVM model = new MemberIndexVM();
            model.ItemViewModels.Add(new MemberIndexItemViewModel { Name = "Test Name 1", MemberNumber = 432, Renewed = false });
            model.ItemViewModels.Add(new MemberIndexItemViewModel { Name = "Test Name 2", MemberNumber = 231, Renewed = true });
            model.ItemViewModels.Add(new MemberIndexItemViewModel { Name = "Test Name 3", MemberNumber = 818, Renewed = true });

            return Task.FromResult(model);
        }

        public Task PostAsync(MemberIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

    public class MemberCreateViewService : IViewService<MemberCreateVM>
    {
        public Task<MemberCreateVM> GetAsync()
        {
            var model = new MemberCreateVM
            {
                MemberProfileItems = Enumerable.Range(0, 5).Select(i => new MemberProfileItemEntity { ItemName = $"item {i}{i}" }).ToList()
            };

            return Task.FromResult(model);
        }

        public Task PostAsync(MemberCreateVM data)
        {
            throw new NotImplementedException();
        }
    }
}
