using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPageBlog.Mvc.Areas.Admin.Conrollers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor,Eyup")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
