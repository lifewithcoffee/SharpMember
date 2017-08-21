using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices
{
    public interface IViewService<TViewModel>
        where TViewModel : class
    {
        Task<TViewModel> GetAsync();
        Task PostAsync(TViewModel data);
    }
}
