using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityIndexViewService
    {
        CommunityIndexVM Get();
        void Post(CommunityIndexVM data);
    }

    public class CommunityIndexViewService : ICommunityIndexViewService
    {
        ICommunityRepository _communityRepository;

        public CommunityIndexViewService(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }

        public CommunityIndexVM Get()
        {
            var commItems = _communityRepository.GetAll().ToList().Select(o => {

                string trim = o.Introduction.Trim();
                string intro;
                if(trim.Length > 150)
                {
                    intro = trim + " ...";
                }
                else
                {
                    intro = trim;
                }

                return new CommunityIndexItemVM { Name = o.Name, Id = o.Id, Introduction = intro };
            }).ToList();
            return  new CommunityIndexVM { ItemViewModels = commItems };
        }

        public void Post(CommunityIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

}
