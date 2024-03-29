﻿namespace StudentsHelper.Web.Infrastructure.Alerts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;

    public class AlertDecoratorResult : IActionResult
    {
        public AlertDecoratorResult(IActionResult result, string type, string body)
        {
            this.Result = result;
            this.Type = type;
            this.Body = body;
        }

        public IActionResult Result { get; }

        public string Type { get; }

        public string Body { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = this.Type;
            tempData["_alert.body"] = this.Body;

            await this.Result.ExecuteResultAsync(context);
        }
    }
}
