using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class MstCustomerController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: MstCustomer
        public ActionResult MstCustomer(string pageMode = "Details")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }

            MstCustomer member = (MstCustomer)Session["Account"];
            MstCustomer model = Models.MstCustomer.GetMstCustomer(member.CustomerId);
            ViewData["PageMode"] = pageMode;
            return View(model);
        }

        // EDIT: MstCustomer
        public ActionResult MstCustomer_Edit(string pageMode = "Edit")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }

            MstCustomer member = (MstCustomer)Session["Account"];
            MstCustomer model = Models.MstCustomer.GetMstCustomer(member.CustomerId);
   
            ViewData["PageMode"] = pageMode;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveCustomerProfile(MstCustomer formData)
        {

            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer member = (MstCustomer)Session["Account"];

            int rowsAffected = -1;

            MstCustomer oriData = EWM.Models.MstCustomer.GetMstCustomer(formData.CustomerId);
            oriData.Email = formData.Email;
            oriData.Address = formData.Address;
            oriData.Postcode = formData.Postcode;
            oriData.State = formData.State;
            oriData.Name = formData.Name;


            rowsAffected = oriData.UpdateMstCustomer(member.Username);

            if (rowsAffected == 1)
            {
                return RedirectToAction("MstCustomer");
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                ViewData["PageMode"] = "Edit";
                return View("MstCustomer_Edit", formData);
            }

        }

     
    }
}