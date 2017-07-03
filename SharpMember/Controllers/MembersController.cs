using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.Models.MemberManagement;

namespace SharpMember.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.MemberProfiles.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfiles
                .SingleOrDefaultAsync(m => m.Id == id);
            if (memberProfile == null)
            {
                return NotFound();
            }

            return View(memberProfile);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberNumber,Renewed,RegisterDate,CeaseDate,Name,Remarks")] MemberProfile memberProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(memberProfile);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfiles.SingleOrDefaultAsync(m => m.Id == id);
            if (memberProfile == null)
            {
                return NotFound();
            }
            return View(memberProfile);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberNumber,Renewed,RegisterDate,CeaseDate,Name,Remarks")] MemberProfile memberProfile)
        {
            if (id != memberProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberProfileExists(memberProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(memberProfile);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfiles
                .SingleOrDefaultAsync(m => m.Id == id);
            if (memberProfile == null)
            {
                return NotFound();
            }

            return View(memberProfile);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberProfile = await _context.MemberProfiles.SingleOrDefaultAsync(m => m.Id == id);
            _context.MemberProfiles.Remove(memberProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MemberProfileExists(int id)
        {
            return _context.MemberProfiles.Any(e => e.Id == id);
        }
    }
}
