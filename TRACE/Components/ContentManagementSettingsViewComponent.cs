using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class ContentManagementSettingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}