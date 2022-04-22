using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWM.HelperClass;
using EWM.Models;

namespace EWM.Controllers
{
    public class MstCustomerController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: MstCustomer
        public ActionResult MstCustomer(string pageMode= "Details")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }

            //ViewBag.Message = "Placeholder Customer Page";
            MstCustomer member = (MstCustomer)Session["Account"];

            ViewData["PageMode"] = pageMode;
            return View(member);
        }

    }
}