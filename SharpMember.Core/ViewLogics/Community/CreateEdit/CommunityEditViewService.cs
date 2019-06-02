﻿using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityEditViewService
    {
        CommunityUpdateVm Get(int commId, int addMore);
        Task PostAsync(CommunityUpdateVm data);
    }

    public class CommunityEditViewService : ICommunityEditViewService
    {
        IRepositoryBase<Community> _communityRepository;
        IRepositoryBase<MemberProfileItemTemplate> _memberProfileItemTemplateRepo;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public CommunityEditViewService(
            IRepositoryBase<Community> orgRepo,
            IRepositoryBase<MemberProfileItemTemplate> memberProfileItemTemplateRepo,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        )
        {
            _communityRepository = orgRepo;
            _memberProfileItemTemplateRepo = memberProfileItemTemplateRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVm Get(int commId, int addMore)
        {
            var community = _communityRepository.GetMany(c => c.Id == commId).Include(c => c.MemberProfileItemTemplates).Single();

            CommunityUpdateVm result = community.ConvertToCommunityUpdateVM();
            result.ItemTemplateVMs = community.MemberProfileItemTemplates.Select(x => new MemberProfileItemTemplateVm { ItemTemplate = x}).ToList();

            for(int i = 0; i < addMore; i++)
            {
                result.ItemTemplateVMs.Add(new MemberProfileItemTemplateVm());
            }

            return result;
        }

        /// <summary>
        /// Unit test: <see cref="U.ViewServices.CommunityViewServiceTests.Community_edit_view_service"/>
        /// </summary>
        public async Task PostAsync(CommunityUpdateVm data)
        {
            _communityRepository.Update(data.ConvertToCommunityWithoutNavProp());
            await _communityRepository.CommitAsync();

            _memberProfileItemTemplateRepo.RemoveRange(data.ItemTemplateVMs.Where(x => x.Delete).Select(x => x.ItemTemplate).ToList());
            _memberProfileItemTemplateRepository.AddOrUpdateItemTemplates(data.Id, data.ItemTemplateVMs.Where(x => !x.Delete && !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Select(x => x.ItemTemplate).ToList());

            await _communityRepository.CommitAsync();
        }
    }
}