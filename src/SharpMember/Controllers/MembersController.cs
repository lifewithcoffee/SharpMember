using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.Repositories;

namespace SharpMember.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepoitory;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepoitory = memberRepository;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _memberRepoitory.GetAll().ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepoitory.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
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
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                _memberRepoitory.Add(member);
                //await _context.SaveChangesAsync();
                await _memberRepoitory.CommitAsync();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var member = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            var member = await _memberRepoitory.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(member);
                    //await _context.SaveChangesAsync();
                    _memberRepoitory.Update(member);
                    await _memberRepoitory.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _memberRepoitory.ExistAsync(e => e.Id == member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var member = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            var member = await _memberRepoitory.GetByIdAsync(id);
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
            //var member = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Members.Remove(member);
            //await _context.SaveChangesAsync();

            var member = await _memberRepoitory.GetByIdAsync(id);
            _memberRepoitory.Delete(member);
            return RedirectToAction("Index");
        }

        //private async Task<bool> MemberExists(int id)
        //{
        //    //return _context.Members.Any(e => e.Id == id);
        //    return await _memberRepoitory.ExistAsync(e => e.Id == id);
        //}
    }
}
