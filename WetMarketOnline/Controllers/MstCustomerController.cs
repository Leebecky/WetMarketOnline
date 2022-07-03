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

        // Customer Home page
        public ActionResult MstCustomer()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }

            return View();
        }

        // Customer Profile page
        public ActionResult CustomerProfile(string pageMode = "Details", string accessMode = "Customer", string customerId = "")
        {
            string profileId = "";
            if (accessMode == "Customer")
            {
                if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
                MstCustomer member = (MstCustomer)Session["Account"];
                profileId = member.CustomerId;
            }

            if (accessMode == "Admin")
            {
                if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }
                profileId = customerId;
            }

            MstCustomer model = Models.MstCustomer.GetMstCustomer(profileId);
            ViewData["PageMode"] = pageMode;
            ViewData["AccessMode"] = accessMode;
            return View(model);
        }

        // EDIT: MstCustomer
        public ActionResult CustomerProfile_Edit(string pageMode = "Edit")
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

            MstCustomer customerData = EWM.Models.MstCustomer.GetMstCustomer(formData.CustomerId);
            customerData.Email = formData.Email;
            customerData.Address = formData.Address;
            customerData.Postcode = formData.Postcode;
            customerData.State = formData.State;
            customerData.Name = formData.Name;
            customerData.Username = formData.Username;


            rowsAffected = customerData.UpdateMstCustomer(member.Username);

            if (rowsAffected == 1)
            {
                return RedirectToAction("CustomerProfile");
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                ViewData["PageMode"] = "Edit";
                return View("CustomerProfile_Edit", formData);
            }

        }

        //? Order List page
        public ActionResult OrderList()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            return View();
        }

        public ActionResult OrderCard_Partial(string status = "")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            List<TxnOrderHdr> orderList = TxnOrderHdr.GetCustomerOrder(user.CustomerId, status);
            orderList = orderList.OrderByDescending(l => l.UpdatedDate).ToList();
            return PartialView(orderList);
        }


        //? Order Tracking page
        public ActionResult OrderTracking(string orderHdrId)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            TxnOrderHdr hdr = TxnOrderHdr.GetTxnOrderHdr(orderHdrId);
            hdr.SetOrderDetails(TxnOrderDtl.GetCompleteOrderDetails(orderHdrId));

            return View(hdr);
        }

        //? AJAX Check Username Availability
        public ActionResult ValidateUsername(string username, string customerId)
        {
            MstCustomer customer = new MstCustomer();
            customer.Username = username;
            List<MstCustomer> customerList = customer.SelectMstCustomer("All");

            if (customerList.Count == 0 || (customerList.Count == 1 && customerList[0].CustomerId == customerId))
            {
                return Json("Ok");
            }
            else
            {
                return Json("Username is in use!");
            }
        }

        ////? Change Password popup
        //public ActionResult ChangePassword_Customer()
        //{
        //    return PartialView();
        //}

        //? Change customer password
        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            MstCustomer customer = Models.MstCustomer.GetMstCustomer(user.CustomerId);
            customer.Password = password;
            customer.UpdateMstCustomer();

            return RedirectToAction("CustomerProfile");
        }
    }
}