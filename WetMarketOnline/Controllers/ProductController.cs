using EWM.Models;
using System;
using System.Collections.Generic;
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
            //MstCategory cat1 = new MstCategory()
            //{
            //    CatLevel = 1
            //};
            //List<MstCategory> catList = cat1.SelectMstCategory("All");

            //if (section != "")
            //{
            //    return new RedirectResult(Url.Action("Product_All") + sectionUrl);
            //}
            List<MstProduct> pList = MstProduct.GetAllCompleteProductData("Active");
            return View(pList);
        }

        // Carousel Sliders - Category 1
        public ActionResult Product_CategorySliderPartial(string catId, string catDesc)
        {
            List<MstProduct> pList = GetProductByCategory(catId);
            ViewData["CatDesc"] = catDesc;
            return PartialView(pList);
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



        //end class
    }
}