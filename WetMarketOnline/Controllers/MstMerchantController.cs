using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWM.HelperClass;

namespace EWM.Controllers
{
    public class MstMerchantController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: MstMerchant
        public ActionResult MstMerchant()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }

            ViewBag.Message = "Placeholder Merchant Page";
            return View();
        }
    }
}