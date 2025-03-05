using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
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

