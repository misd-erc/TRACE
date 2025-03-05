using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class UserManagementSettingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
