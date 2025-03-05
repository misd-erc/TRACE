using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class NotificationsSettingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}