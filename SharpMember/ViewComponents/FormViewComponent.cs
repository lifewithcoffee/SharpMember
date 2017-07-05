using Microsoft.AspNetCore.Mvc;
using SharpMember.Core.Data.Models.MemberManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpMember.ViewComponents
{
    public class FormViewComponentModel
    {
        public Type Type { get; set; }
        public bool IsCreate { get; set; }
    }

    [ViewComponent]
    public class Form : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(Type type, bool IsCreate)
        {
            return Task.FromResult<IViewComponentResult>(View(new FormViewComponentModel { Type = type, IsCreate = IsCreate }));
        }
    }
}
