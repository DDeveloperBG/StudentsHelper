﻿namespace StudentsHelper.Web.Infrastructure.Alerts
{
    using Microsoft.AspNetCore.Mvc;

    public static class AlertExtensions
    {
        public static IActionResult WithSuccess(this IActionResult result, string body)
        {
            return Alert(result, "success", body);
        }

        public static IActionResult WithInfo(this IActionResult result, string body)
        {
            return Alert(result, "info", body);
        }

        public static IActionResult WithWarning(this IActionResult result, string body)
        {
            return Alert(result, "warning", body);
        }

        public static IActionResult WithDanger(this IActionResult result, string body)
        {
            return Alert(result, "danger", body);
        }

        private static IActionResult Alert(IActionResult result, string type, string body)
        {
            return new AlertDecoratorResult(result, type, body);
        }
    }
}
