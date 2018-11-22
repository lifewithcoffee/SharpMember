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
using Microsoft.Extensions.DependencyInjection;

namespace SharpMember.Controllers
{
    //[Authorize]
    public class CommunitiesController : ControllerBase
    {
        ILogger<CommunitiesController> _logger;
        UserManager<ApplicationUser> _userManager;

        public CommunitiesController(ILogger<CommunitiesController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Communities
        public ActionResult Index()
        {
            var vs = GetService<ICommunityIndexViewService>();
            return View(vs.Get());
        }

        // GET: Communities/Create
        [HttpGet]
        public ActionResult Create()
        {
            var vs = GetService<ICommunityCreateViewService>();
            return View(vs.Get());
        }

        // POST: Communities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CommunityUpdateVm data)
        {
            var vs = GetService<ICommunityCreateViewService>();
            try
            {
                if (ModelState.IsValid)
                {
                    string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                    int returnedOrgId = await vs.PostAsync(appUserId, data);
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
            var vs = GetService<ICommunityEditViewService>();
            try
            {
                return View(vs.Get(id, addMore));
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
            var vs = GetService<ICommunityEditViewService>();
            try
            {
                //string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                await vs.PostAsync(data);

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
            var vs = GetService<ICommunityMembersViewService>();
            return View(vs.Get(commId));
        }

        [HttpPost("/[controller]/{commId}/members")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Members(CommunityMembersVm model, int commId)
        {
            var vs = GetService<ICommunityMembersViewService>();
            await vs.PostToDeleteSelected(model);
            return RedirectToAction(nameof(Members), new { commId = commId });
        }

        [Route("/[controller]/{commId}/groups")]
        public ActionResult<CommunityGroupsVm> Groups(int commId)
        {
            var vs = GetService<ICommunityGroupsViewService>();
            return View(vs.Get(commId));
        }

        [HttpPost("/[controller]/{commId}/groups")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Groups(CommunityGroupsVm model, int commId)
        {
            var vs = GetService<ICommunityGroupsViewService>();
            await vs.PostToDeleteSelected(model);
            return RedirectToAction(nameof(Groups), new { commId = commId });
        }
    }
}