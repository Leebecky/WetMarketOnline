using EWM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductAll(string section = "")
        {
            string sectionUrl = "#" + section;
            List<MstProduct> pList = MstProduct.GetAllCompleteProductData("Active");

            MstCategory cat1 = new MstCategory()
            {
                CatLevel = 1
            };
            List<MstCategory> catList = cat1.SelectMstCategory("All");
            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            ViewData["CatList"] = catList;
            return View(pList);
        }

        public ActionResult ProductCard_Partial(string selectedCategories = "")
        {

            List<MstProduct> productList = MstProduct.GetAllCompleteProductData("Active");

            if (!string.IsNullOrEmpty(selectedCategories))
            {
                string[] catList = (string[])Newtonsoft.Json.JsonConvert.DeserializeObject(selectedCategories, typeof(string[]));
                
                productList = MstProduct.GetProductByCategory(catList);
            }

            return PartialView(productList);
        }

        // Product Filters
        public ActionResult ProductFilter_Partial()
        {
            MstCategory cat1 = new MstCategory()
            {
                CatLevel = 1
            };
            List<MstCategory> catList = cat1.SelectMstCategory("All");

            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            return PartialView(catList);
        }

        // Single Product Item display
        public ActionResult ProductSingle(string productId)
        {
            MstProduct product = MstProduct.GetCompleteProductData(productId, "Active");
            return View(product);
        }


        public List<MstProduct> GetProductByCategory(string catId = "")
        {
            List<MstProduct> allProducts = MstProduct.GetAllCompleteProductData("Active");

            allProducts.RemoveAll(i => i.GetCatList().FindIndex(c => c.CategoryId == catId) == -1);
            return allProducts;
        }


        //? AJAX call for Parent Cat dropdown
        public ActionResult GetParentCat_Cb(int catLevel = 1, string parentId = "", string selectedData = "")
        {
            MstCategory catSearch = new MstCategory();
            catSearch.CatLevel = catLevel;
            catSearch.ParentCatId = parentId;
            List<MstCategory> parentList = catSearch.SelectMstCategory("All");
            List<SelectListItem> selectionList = new List<SelectListItem>();

            for (int i = 0; i < parentList.Count; i++)
            {
                MstCategory item = parentList[i];
                SelectListItem selectionItem = new SelectListItem();
                selectionItem.Text = item.CategoryCode;
                selectionItem.Value = item.CategoryId;

                //if (pageMode == "New" && i == 0 && catLevel == 1)
                //{
                //    selectionItem.Selected = true;
                //}

                if (selectedData == item.CategoryId)
                {
                    selectionItem.Selected = true;
                }
                selectionList.Add(selectionItem);
            }

            return Json(selectionList);
        }

        //end class
    }
}