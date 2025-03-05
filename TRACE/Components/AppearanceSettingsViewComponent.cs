using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class AppearanceSettingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}