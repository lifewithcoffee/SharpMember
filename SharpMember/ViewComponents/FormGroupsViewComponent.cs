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
        public string[] HidenFields { get; set; } = { };
        public string[] ReadonlyFields { get; set; } = { };
        public string[] TextAreaFields { get; set; } = { };
    }

    public class FormGroupsViewComponent: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(Type entityType, string hiddenFields, string readonlyFields, string textAreaFields)
        {
            FormGroupsVcModel model = new FormGroupsVcModel {
                Type = entityType
            };

            if (hiddenFields != null)
            {
                model.HidenFields = hiddenFields.ToLower().Split(';');
            }

            if (readonlyFields != null)
            {
                model.ReadonlyFields = readonlyFields.ToLower().Split(';');
            }

            if (textAreaFields != null)
            {
                model.TextAreaFields = textAreaFields.ToLower().Split(';');
            }

            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
