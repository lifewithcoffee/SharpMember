using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupCreateViewService
    {
        Task<GroupUpdateVM> GetAsync(int orgId, string appUserId);
        Task<int> Post(MemberUpdateVM data);
    }

    public class GroupCreateViewService : IGroupCreateViewService
    {
        public Task<GroupUpdateVM> GetAsync(int orgId, string appUserId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Post(MemberUpdateVM data)
        {
            throw new NotImplementedException();
        }
    }
}
