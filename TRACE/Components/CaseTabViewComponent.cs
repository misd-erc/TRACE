﻿using Microsoft.AspNetCore.Mvc;

namespace TRACE.Components
{
    public class MyCasesMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string currentPath = HttpContext.Request.Path.ToString().ToLower();
            string activeTab = "";

            if (currentPath.Contains("lettercomplaints"))
            {
                activeTab = "_loc";
            }
            else if (currentPath.Contains("docketedcases"))
            {
                activeTab = "_dc";
            }
            else if (currentPath.Contains("cocres"))
            {
                activeTab = "_cocres";
            }

            ViewData["ActiveTab"] = activeTab;
            return View();
        }
    }
}
