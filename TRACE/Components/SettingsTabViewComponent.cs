using Microsoft.AspNetCore.Mvc;

namespace TRACE.Components
{
    public class SettingsTabViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string activeTab)
        {
            ViewData["ActiveTab"] = activeTab;
            return View();
        }
    }
}

