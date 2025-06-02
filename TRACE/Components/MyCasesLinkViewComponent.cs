using Microsoft.AspNetCore.Mvc;
using TRACE.Helpers;
using System.Threading.Tasks;

namespace TRACE.Components
{
    public class MyCasesLinkViewComponent : ViewComponent
    {
        private readonly CurrentUserHelper _currentUserHelper;

        public MyCasesLinkViewComponent(CurrentUserHelper currentUserHelper)
        {
            _currentUserHelper = currentUserHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var department = await _currentUserHelper.GetDepartmentAsync();
            string action = department == "MOS" ? "cocres" : "docketedcases";
            return View("Default", action);
        }
    }
}
