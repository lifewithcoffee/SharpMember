using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.ViewComponents
{
    public class FormGroupsVcModel
    {
        public Type Type { get; set; }
        public object Instance { get; set; }
        public string[] HidenFields { get; set; } = { };
        public string[] ReadonlyFields { get; set; } = { };
        public string[] TextAreaFields { get; set; } = { };
    }

    public class FormGroupsViewComponent: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(Type entityType, object instance, string hiddenFields, string readonlyFields, string textAreaFields)
        {
            FormGroupsVcModel model = new FormGroupsVcModel {
                Type = entityType,
                Instance = instance
            };

            if (hiddenFields != null)
            {
                model.HidenFields = hiddenFields.ToLower().Split(',').Select(t => t.Trim()).ToArray();
            }

            if (readonlyFields != null)
            {
                model.ReadonlyFields = readonlyFields.ToLower().Split(',').Select(t => t.Trim()).ToArray();
            }

            if (textAreaFields != null)
            {
                model.TextAreaFields = textAreaFields.ToLower().Split(',').Select(t => t.Trim()).ToArray();
            }

            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
