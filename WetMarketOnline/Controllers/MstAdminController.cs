using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWM.Models;

namespace EWM.Controllers
{
    public class MstAdminController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //? MstAdmin Home Page
        public ActionResult MstAdmin()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            ViewBag.Message = "Placeholder Admin Page";
            return View();
        }

  

    }
}