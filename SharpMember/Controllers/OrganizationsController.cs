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

namespace SharpMember.Controllers
{
    public class OrganizationsController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        IOrganizationIndexViewService _organizationIndexViewService;
        IOrganizationCreateViewService _organizationCreateViewService;

        public OrganizationsController(
            UserManager<ApplicationUser> userManager,
            IOrganizationIndexViewService organizationIndexViewService,
            IOrganizationCreateViewService organizationCreateViewService
        ){
            _userManager = userManager;
            _organizationIndexViewService = organizationIndexViewService;
            _organizationCreateViewService = organizationCreateViewService;
        }

        // GET: Organizations
        public ActionResult Index()
        {
            return View(_organizationIndexViewService.Get());
        }

        // GET: Organizations/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(_organizationCreateViewService.Get());
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationCreateVM data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string appUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                    await _organizationCreateViewService.Post(appUserId, data);
                    return RedirectToAction("Index");
                }
                return View(data);
            }
            catch
            {
                return View(data);
            }
        }

        // GET: Organizations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organizations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Organizations/Delete/5
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
    }
}