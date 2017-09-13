using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models.MemberSystem;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Definitions;
using SharpMember.Core.Views.ViewServices.GroupViewServices;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Definitions;

namespace SharpMember.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        IAuthorizationService _authorizationService;
        IGroupCreateViewService _groupCreateViewService;
        IGroupEditViewService _groupEditViewService;

        public GroupsController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            IGroupCreateViewService groupCreateViewService,
            IGroupEditViewService groupEditViewService
        ){
            _context = context;
            _authorizationService = authorizationService;
            _groupCreateViewService = groupCreateViewService;
            _groupEditViewService = groupEditViewService;
        }

        // GET: Groups/Create
        public IActionResult Create(int commId)
        {
            return View(_groupCreateViewService.GetAsync(commId));
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupUpdateVM group)
        {
            if (ModelState.IsValid)
            {
                int newGroupId = await _groupCreateViewService.Post(group);
                return RedirectToAction(nameof(Edit), new { Id = newGroupId });
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int? id)
        {
            //await this._authorizationService.AuthorizeAsync(User, new Group(), PolicyName.RequireRoleOf_GroupOwner);

            if (id == null)
            {
                return NotFound();
            }

            var model = _groupEditViewService.Get(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupUpdateVM group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _groupEditViewService.PostAsync(group);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", new { id = id });
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(_ => _.Community)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            int commId = group.CommunityId;
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CommunitiesController.Groups), ControllerNames.Communities, new { commId = commId });
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
