using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class MstCategoryController : Controller
    {

        //? MstAdmin Manage Category Page
        public ActionResult ManageCategory()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            ViewBag.Error = TempData["Error"];
            return View();
        }

        // GET: MstCategory/Details/5
        public ActionResult Details(string id, string mode, int catLevel = 1)
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

            ViewData["ParentList"] = selectionList;
            ViewData["PageMode"] = mode;

            if (mode == "New")
            {
                return View("MstCategory_Details", new MstCategory());
            }
            else
            {
                MstCategory categoryData = MstCategory.GetMstCategory(id);
                if (catLevel > 1)
                {
                    MstCategory parent = MstCategory.GetMstCategory(categoryData.ParentCatId);
                    categoryData.ParentCatId = parent.CategoryId;
                }
                return View("MstCategory_Details", categoryData);
            }
        }

        [HttpPost]
        public ActionResult UpdateMstCategory(MstCategory formData, string mode)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }
            MstAdmin member = (MstAdmin)Session["Account"];

            int rowsAffected = -1;

            if (mode == "New")
            {
                rowsAffected = formData.CreateMstCategory(member.Username);
            }
            else if (mode == "Edit")
            {
                MstCategory oriData = MstCategory.GetMstCategory(formData.CategoryId);
                oriData.CategoryCode = formData.CategoryCode;
                oriData.CategoryDesc = formData.CategoryDesc;
                oriData.CatLevel = formData.CatLevel;
                oriData.ParentCatId = formData.ParentCatId;
                oriData.Status = formData.Status;

                rowsAffected = oriData.UpdateMstCategory(member.Username);
            }

            if (rowsAffected == 1)
            {
                return RedirectToAction("ManageCategory");
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                ViewData["PageMode"] = mode;
                return View("MstCategory_Details", formData);
            }

        }

        /// GET: MstCategory/Delete/5
        public ActionResult Delete(string id)
        {
            MstCategory deleteCat = MstCategory.GetMstCategory(id);
            List<MstCategory> checkChildren = MstCategory.GetCategoryDescendents(deleteCat.CatLevel.GetValueOrDefault(), deleteCat.CategoryId);

            if (checkChildren.Count > 0)
            {
                TempData["Error"] = "This category has children. Unable to delete category.";
                return RedirectToAction("ManageCategory");
            }

            int rowsAffected = deleteCat.DeleteMstCategory();

            if (rowsAffected != 1)
            {
                TempData["Error"] = "Failed to delete the category. Please try again.";
            }
            return RedirectToAction("ManageCategory");
        }

        public ActionResult ManageCategory_Level(int catLevel = 1)
        {
            //MstCategory catSearch = new MstCategory();
            //catSearch.CatLevel = catLevel;
            //List<MstCategory> list = catSearch.SelectMstCategory("All");
            List<MstCategory> list = MstCategory.GetCategoryDescendents(catLevel);
            list = list.OrderBy(l => l.CategoryCode).ToList();
            return PartialView(list);

        }

        //? AJAX call for Parent Cat dropdown
        public ActionResult GetParentCat_Cb(int catLevel = 1, string parentCat = "")
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

        //? AJAX Check Categpry Code Validity
        public ActionResult ValidateCategory(string catCode, string catId)
        {
            MstCategory cat = new MstCategory();
            cat.CategoryCode = catCode;
            List<MstCategory> catList = cat.SelectMstCategory("All");

            if (catList.Count == 0 || (catList.Count == 1 && catList[0].CategoryId == catId))
            {
                return Json("Ok");
            }
            else
            {
                return Json("Category Code is in use!");
            }
        }

        //end class
    }
}
