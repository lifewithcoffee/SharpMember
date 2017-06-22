using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpMember.Services.Excel;
using SharpMember.Core.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace SharpMember.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public HomeController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About([FromServices] IMemberRepository memberService)
        {
            ViewData["Message"] = "Your application description page!";

            memberService.ImportFromExcel();

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            //IMemberRepository memberSvc1 = _serviceProvider.GetService<IMemberRepository>();
            //IMemberRepository memberSvc2 = _serviceProvider.GetService<IMemberRepository>();

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
