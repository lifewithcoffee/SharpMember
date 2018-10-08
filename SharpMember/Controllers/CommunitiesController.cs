using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewServices;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;
using SharpMember.Core.Definitions;
using SharpMember.Core.Views.ViewModels.CommunityVms;

namespace SharpMember.Controllers
{
    //[Authorize]
    public class CommunitiesController : Controller
    {
        ILogger<CommunitiesController> _logger;
        UserManager<ApplicationUser> _userManager;
        ICommunityIndexViewService _communityIndexViewService;
        ICommunityCreateViewService _communityCreateViewService;
        ICommunityEditViewService _communityEditViewService;
        ICommunityMembersViewService _memberIndexViewService;
        ICommunityGroupsViewService _communityGroupsViewService;

        public CommunitiesController(
            ILogger<CommunitiesController> logger,
            UserManager<ApplicationUser> userManager,
            ICommunityIndexViewService communityIndexViewService,
            ICommunityCreateViewService communityCreateViewService,
            ICommunityEditViewService communityEditViewService,
            ICommunityMembersViewService memberIndexViewService,
            ICommunityGroupsViewService communityGroupsViewService
        ){
            _logger = logger;
            _userManager = userManager;
            _communityIndexViewService = communityIndexViewService;
            _communityCreateViewService = communityCreateViewService;
            _communityEditViewService = communityEditViewService;
            _memberIndexViewService = memberIndexViewService;
            _communityGroupsViewService = communityGroupsViewService;
        }

        // GET: Communities
        public ActionResult Index()
        {
            return View(_communityIndexViewService.Get());
        }

        // GET: Communities/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(_communityCreateViewService.Get());
        }

        // POST: Communities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CommunityUpdateVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                    int returnedOrgId = await _communityCreateViewService.PostAsync(appUserId, data);
                    return RedirectToAction(nameof(Edit), new { id = returnedOrgId });
                }
                return View(data);
            }
            catch
            {
                return View(data);
            }
        }

        // GET: Communities/Edit/5
        public ActionResult Edit(int id, int addMore = 0)
        {
            try
            {
                return View(_communityEditViewService.Get(id, addMore));
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: Communities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CommunityUpdateVm data, int id, string command, int addMore = 0)
        {
            try
            {
                //string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                await _communityEditViewService.PostAsync(data);

                if (command == PostCommandNames.AddMoreItemTemplates)
                    addMore = 10;

                return RedirectToAction(nameof(Edit), new { id = id, addMore = addMore });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(data);
            }
        }

        // GET: Communities/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Communities/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Route("/[controller]/{commId}/members")]
        public ActionResult Members(int commId)
        {
            return View(_memberIndexViewService.Get(commId));
        }

        [HttpPost("/[controller]/{commId}/members")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Members(CommunityMembersVm model, int commId)
        {
            await _memberIndexViewService.PostToDeleteSelected(model);
            return RedirectToAction(nameof(Members), new { commId = commId });
        }

        [Route("/[controller]/{commId}/groups")]
        public ActionResult<CommunityGroupsVm> Groups(int commId)
        {
            return View(_communityGroupsViewService.Get(commId));
        }

        [HttpPost("/[controller]/{commId}/groups")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Groups(CommunityGroupsVm model, int commId)
        {
            await _communityGroupsViewService.PostToDeleteSelected(model);
            return RedirectToAction(nameof(Groups), new { commId = commId });
        }
    }
}