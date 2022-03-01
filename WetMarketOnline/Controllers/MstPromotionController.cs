using EWM.HelperClass;
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

            ViewBag.Message = "Placeholder Admin Page";
            return View();
        }

        // GET: MstPromotion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MstPromotion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MstPromotion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MstPromotion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MstPromotion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MstPromotion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MstPromotion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
