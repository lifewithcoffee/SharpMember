using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data;
using SharpMember.Core.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Views.ViewServices.MemberViewServices;
using SharpMember.Core.Views.ViewServices.CommunityViewServices;

namespace SharpMember.Controllers
{
    //[Authorize]
    public class MembersController : Controller
    {
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        IMemberCreateViewService _memberCreateViewService;
        IMemberEditViewService _memberEditViewService;

        public MembersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICommunityMembersViewService memberIndexViewService,
            IMemberCreateViewService memberCreateViewService,
            IMemberEditViewService memberEditViewService
        ){
            _context = context;
            _userManager = userManager;
            _memberCreateViewService = memberCreateViewService;
            _memberEditViewService = memberEditViewService;
        }

        // GET: Members/Create
        public async Task<IActionResult> Create(int commId)
        {
            string appUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(await this._memberCreateViewService.GetAsync(commId, appUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberUpdateVM data)
        {
            if (ModelState.IsValid)
            {
                int id = await this._memberCreateViewService.Post(data);
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return View(data);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _memberEditViewService.GetAsync(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberUpdateVM data)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _memberEditViewService.PostAsync(data);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(data.Id))
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
            return View(data);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .SingleOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
