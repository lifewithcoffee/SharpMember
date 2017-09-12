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

namespace SharpMember.Controllers
{
    [Authorize]
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
            ICommunityMembersViewService memberIndexViewService
        ){
            _logger = logger;
            _userManager = userManager;
            _communityIndexViewService = communityIndexViewService;
            _communityCreateViewService = communityCreateViewService;
            _communityEditViewService = communityEditViewService;
            _memberIndexViewService = memberIndexViewService;
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
        public async Task<ActionResult> Create(CommunityUpdateVM data)
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
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_communityEditViewService.Get(id));
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: Communities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CommunityUpdateVM data)
        {
            try
            {
                //string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                await _communityEditViewService.PostAsync(data);

                return RedirectToAction(nameof(Edit), new { id = id });
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
        public async Task<IActionResult> Members(CommunityMembersVM model, int commId)
        {
            await _memberIndexViewService.PostToDeleteSelected(model);
            return View(model);
        }

        [Route("/[controller]/{commId}/groups")]
        public ActionResult Groups(int commId)
        {
            return View();
        }

        [HttpPost("/[controller]/{commId}/groups")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Groups(object model, int commId)
        {
            return View();
        }
    }
}