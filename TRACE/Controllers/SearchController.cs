using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TRACE.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        //MODULAR SEARCH NI ALDRIN
        private static readonly List<ModuleModel> modules = new List<ModuleModel>
        {
            //CASE RELATED
            new ModuleModel { Name = "<b>Case:</b> All Cases", Link = "/allcases" },
            new ModuleModel { Name = "<b>Case:</b> Letter Complaints", Link = "/lettercomplaints" },
            new ModuleModel { Name = "<b>Case:</b> Docketed Cases", Link = "/docketedcases" },

            //DOCUMENT RELATED
            new ModuleModel { Name = "<b>Document:</b> Document Management", Link = "/documents" },

            //DOCUMENT RELATED
            new ModuleModel { Name = "<b>Hearing:</b> Hearings", Link = "/hearings" },

            //CONTENT RELATED
            new ModuleModel { Name = "<b>Content:</b> Content Management", Link = "/contentmanagement" },

            //CONTENT RELATED
            new ModuleModel { Name = "<b>User:</b> User Management", Link = "/usermanagement" },

            //NOTIFICATION RELATED
            new ModuleModel { Name = "<b>Notification:</b> Notifications", Link = "/notifications" },

            //Settings RELATED
            new ModuleModel { Name = "<b>Settings:</b> Notification Settings", Link = "/settings" },
            new ModuleModel { Name = "<b>Settings:</b> Appearance Settings", Link = "/settings" },
            new ModuleModel { Name = "<b>Settings:</b> Theme Settings", Link = "/settings" }
        };

        [HttpGet]
        public JsonResult SearchModules(string query)
        {
            var filteredModules = string.IsNullOrWhiteSpace(query)
                ? new List<ModuleModel>()
                : modules.Where(m => m.Name.ToLower().Contains(query.ToLower())).ToList();

            return Json(filteredModules);
        }
    }

    public class ModuleModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
