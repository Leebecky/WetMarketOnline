using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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



        //? MstMerchant Manage Product Page
        public ActionResult ManageProduct()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant user = (MstMerchant)Session["Account"];

            MstProduct productSearch = new MstProduct();
            productSearch.MerchantId = user.MerchantId;
            List<MstProduct> list = productSearch.SelectMstProduct("All");

            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            ViewBag.Error = TempData["Error"];
            return View(list);
        }

        // GET: MstProduct/Details/5
        public ActionResult Details(string id, string mode, int productLevel = 1)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstProduct productSearch = new MstProduct();
            //productSearch.CatLevel = productLevel - 1;
            //List<MstProduct> parentList = productSearch.SelectMstProduct("All");
            //List<SelectListItem> selectionList = new List<SelectListItem>();

            //foreach (var item in parentList)
            //{
            //    SelectListItem selectionItem = new SelectListItem();
            //    selectionItem.Text = item.ProductCode;
            //    selectionItem.Value = item.ProductId;
            //    selectionList.Add(selectionItem);
            //}

            //ViewData["ParentList"] = selectionList;
            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            ViewData["PageMode"] = mode;

            if (mode == "New")
            {
                return View("MstProduct_Details", new MstProduct());
            }
            else
            {
                MstProduct productData = MstProduct.GetMstProduct(id);
                //if (productLevel > 1)
                //{
                //    MstProduct parent = MstProduct.GetMstProduct(productData.ParentCatId);
                //    productData.ParentCatId = parent.ProductCode;
                //}
                return View("MstProduct_Details", productData);
            }
        }

        [HttpPost]
        public ActionResult UpdateMstProduct(MstProduct formData, string mode)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant member = (MstMerchant)Session["Account"];

            int rowsAffected = -1;

            if (mode == "New")
            {
                rowsAffected = formData.CreateMstProduct(member.Username);
            }
            else if (mode == "Edit")
            {
                MstProduct oriData = MstProduct.GetMstProduct(formData.ProductId);
                oriData.ProductCode = formData.ProductCode;
                oriData.ProductName = formData.ProductName;
                oriData.ProductDesc = formData.ProductDesc;
                oriData.Price = formData.Price;
                oriData.Quantity = formData.Quantity;
                oriData.Status = formData.Status;

                rowsAffected = oriData.UpdateMstProduct(member.Username);
            }

            if (rowsAffected == 1)
            {
                return RedirectToAction("ManageProduct");
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                ViewData["PageMode"] = mode;
                return View("MstProduct_Details", formData);
            }

        }

        /// GET: MstProduct/Delete/5
        public ActionResult Delete(string id)
        {
            MstProduct deleteCat = MstProduct.GetMstProduct(id);

            int rowsAffected = deleteCat.DeleteMstProduct();

            if (rowsAffected != 1)
            {
                TempData["Error"] = "Failed to delete the product. Please try again.";
            }
            return RedirectToAction("ManageProduct");
        }

        //? AJAX call for Parent Cat dropdown
        public ActionResult GetParentCat_Cb(int catLevel = 1)
        {
            MstCategory catSearch = new MstCategory();
            catSearch.CatLevel = catLevel - 1;
            List<MstCategory> parentList = catSearch.SelectMstCategory("All");
            List<SelectListItem> selectionList = new List<SelectListItem>();

            foreach (var item in parentList)
            {
                SelectListItem selectionItem = new SelectListItem();
                selectionItem.Text = item.CategoryCode;
                selectionItem.Value = item.CategoryId;
                selectionList.Add(selectionItem);
            }

            return Json(selectionList);
        }

        //end class
    }
}