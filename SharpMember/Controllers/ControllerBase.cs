using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SharpMember.Controllers
{
    public class ControllerBase : Controller
    {
        public T GetService<T>()
        {
            return HttpContext.RequestServices.GetService<T>();
        }
    }
}
