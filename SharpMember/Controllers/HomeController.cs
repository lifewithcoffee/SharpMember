using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpMember.Core.Services.Excel;
using SharpMember.Core.Data.DataServices;
using Microsoft.Extensions.DependencyInjection;
using SharpMember.Models;

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
            ViewData["Message"] = "Your application description page.";

            excelReadService.ImportFromExcel(@"C:\_temp\test.xlsx");

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
