using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices
{
    public interface IMemberIndexViewService
    {
        MemberIndexVM Get();
        void Post(MemberIndexVM data);
    }

    public class MemberIndexViewService : IMemberIndexViewService
    {
        public MemberIndexVM Get()
        {
            MemberIndexVM model = new MemberIndexVM();
            model.ItemViewModels.Add(new MemberIndexItemVM { Name = "Test Name 1", MemberNumber = 432, Renewed = false });
            model.ItemViewModels.Add(new MemberIndexItemVM { Name = "Test Name 2", MemberNumber = 231, Renewed = true });
            model.ItemViewModels.Add(new MemberIndexItemVM { Name = "Test Name 3", MemberNumber = 818, Renewed = true });

            return model;
        }

        public void Post(MemberIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMemberCreateViewService
    {
        MemberCreateVM Get();
        void Post(MemberCreateVM data);
    }

    public class MemberCreateViewService : IMemberCreateViewService
    {
        public MemberCreateVM Get()
        {
            var model = new MemberCreateVM
            {
                MemberProfileItems = Enumerable.Range(0, 5).Select(i => new MemberProfileItemEntity { ItemName = $"item {i}{i}" }).ToList()
            };

            return model;
        }

        public void Post(MemberCreateVM data)
        {
            throw new NotImplementedException();
        }
    }
}
