using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityIndexHandler
    {
        CommunityIndexVm Get();
        void Post(CommunityIndexVm data);
    }

    public class CommunityIndexHandler : ICommunityIndexHandler
    {
        IRepository<Community> _communityRepository;

        public CommunityIndexHandler(IRepository<Community> communityRepository)
        {
            _communityRepository = communityRepository;
        }

        public CommunityIndexVm Get()
        {
            var commItems = _communityRepository.QueryAll().ToList().Select(o => {

                string trim = o.Introduction != null? o.Introduction.Trim() : "";
                string intro;
                if(trim.Length > 150)
                    intro = trim + " ...";
                else
                    intro = trim;

                return new CommunityIndexItemVm { Name = o.Name, Id = o.Id, Introduction = intro };
            }).ToList();
            return  new CommunityIndexVm { ItemViewModels = commItems };
        }

        public void Post(CommunityIndexVm data)
        {
            throw new NotImplementedException();
        }
    }

}
