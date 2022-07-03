using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class MstAdminController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //? MstAdmin Home Page
        public ActionResult MstAdmin()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            //ViewBag.Message = "Placeholder Admin Page";
            return View();
        }

        //? Order List page
        public ActionResult OrderList_Admin()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }
            return View();
        }

        public ActionResult OrderCard_AdminPartial(string status = "")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            List<TxnOrderHdr> orderList = TxnOrderHdr.GetAllOrders(status);
            orderList = orderList.OrderByDescending(l => l.UpdatedDate).ToList();
            return PartialView(orderList);
        }


        //? Order Tracking page
        public ActionResult OrderTracking_Admin(string orderHdrId)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            TxnOrderHdr hdr = TxnOrderHdr.GetTxnOrderHdr(orderHdrId);
            hdr.SetOrderDetails(TxnOrderDtl.GetCompleteOrderDetails(orderHdrId));

            return View(hdr);
        }


        //? AJAX - Update Order Status
        public ActionResult UpdateOrderStatus(string status, string orderHdrId)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }
            MstAdmin user = (MstAdmin)Session["Account"];

            TxnOrderHdr order = TxnOrderHdr.GetTxnOrderHdr(orderHdrId);
            order.Status = status;
            order.UpdateTxnOrderHdr(user.Username);

            return Json("Ok");
        }

    }
}