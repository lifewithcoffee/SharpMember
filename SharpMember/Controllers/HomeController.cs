using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpMember.Core.Services.Excel;
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

        public IActionResult About([FromServices] IZjuaaaMemberExcelFileReadService excelReadService)
        {
            ViewData["Message"] = "Your application description page!";

            excelReadService.ImportFromExcel(@"C:\_temp\test.xlsx");

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
