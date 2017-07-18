using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharpMember.Core.Views.ViewModels;

namespace SharpMember.Controllers
{
    public class OrganizationsController : Controller
    {
        // GET: Organizations
        public ActionResult Index()
        {
            return View();
        }

        // GET: Organizations/Create
        [HttpGet]
        public ActionResult Create()
        {
            OrganizationCreateViewModel model = new OrganizationCreateViewModel {
                MemberProfileItemTemplates = Enumerable.Range(0,5).Select( i => new MemberProfileItemTemplate() ).ToList()
            };
            return View(model);
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationCreateViewModel data)
        {
            try
            {
                // TODO: Add insert logic here

                //return RedirectToAction("Index");
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