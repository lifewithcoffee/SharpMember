using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupEditViewService
    {
        Task<GroupUpdateVM> GetAsync(int id);
        Task PostAsync(GroupUpdateVM data);
    }

    public class GroupEditViewService : IGroupEditViewService
    {
        public Task<GroupUpdateVM> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task PostAsync(GroupUpdateVM data)
        {
            throw new NotImplementedException();
        }
    }
}
