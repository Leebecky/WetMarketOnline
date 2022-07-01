using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class MstPromotionController : Controller
    {

        //? MstAdmin Manage Promotion Page
        public ActionResult ManagePromotion()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            List<MstPromotion> list = MstPromotion.SelectMstPromotion_All();
            return View(list);
        }

        // GET: MstPromotion/Details/5
        public ActionResult Details(string id, string mode)
        {

            ViewData["PageMode"] = mode;
            if (mode == "New")
            {
                return View("MstPromotion_Details", new MstPromotion());
            }
            else
            {
                MstPromotion slider = MstPromotion.GetMstPromotion(id);
                return View("MstPromotion_Details", slider);
            }
        }

        [HttpPost]
        public ActionResult UpdateMstPromotion(MstPromotion formData, string mode)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }
            MstAdmin member = (MstAdmin)Session["Account"];

            int rowsAffected = -1;

            if (mode == "New")
            {
                rowsAffected = formData.CreateMstPromotion(member.Username);
            }
            else if (mode == "Edit")
            {
                MstPromotion oriData = MstPromotion.GetMstPromotion(formData.PromotionId);
                oriData.PromotionCode = formData.PromotionCode;
                oriData.PromotionDesc = formData.PromotionDesc;
                oriData.StartDate = formData.StartDate;
                oriData.EndDate = formData.EndDate;
                oriData.Status = formData.Status;
                oriData.Amount = formData.Amount;

                rowsAffected = oriData.UpdateMstPromotion(member.Username);
            }

            if (rowsAffected == 1)
            {
                return RedirectToAction("ManagePromotion");
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                ViewData["PageMode"] = mode;
                return View("MstPromotion_Details", formData);
            }

        }

        /// GET: MstPromotion/Delete/5
        public ActionResult Delete(string id)
        {
            MstPromotion promo = MstPromotion.GetMstPromotion(id);
            int rowsAffected = promo.DeleteMstPromotion();


            if (rowsAffected == 1)
            {
                return RedirectToAction("ManagePromotion");
            }
            else
            {
                ViewBag.Error = "Failed to delete the promotion. Please try again.";
                return View();
            }
        }

        //? AJAX Check Promotion Code Validity
        public ActionResult ValidatePromotion(string promoCode)
        {
            MstPromotion promo = new MstPromotion();
            promo.PromotionCode = promoCode;
            int count = promo.CheckMstPromotion();

            if (count > 0)
            {
                return Json("Promotion Code is in use!");
            } else
            {
                return Json("Ok");
            }
        }

    }
}
