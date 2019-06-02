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
using Microsoft.Extensions.DependencyInjection;

namespace SharpMember.Controllers
{
    //[Authorize]
    public class MembersController : ControllerBase
    {
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public MembersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager
        ){
            _context = context;
            _userManager = userManager;
        }

        // GET: Members/Create
        public async Task<IActionResult> Create(int commId)
        {
            var vs = HttpContext.RequestServices.GetService<IMemberCreateHandler>();

            string appUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(await vs.GetAsync(commId, appUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberUpdateVm data)
        {
            var vs = HttpContext.RequestServices.GetService<IMemberCreateHandler>();

            if (ModelState.IsValid)
            {
                int id = await vs.Post(data);
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return View(data);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vs = GetService<IMemberEditHandler>();
            if (id == null)
            {
                return NotFound();
            }

            var model = await vs.GetAsync(id.Value);

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
        public async Task<IActionResult> Edit(int id, MemberUpdateVm data)
        {
            var vs = GetService<IMemberEditHandler>();
            if (id != data.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await vs.PostAsync(data);
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
