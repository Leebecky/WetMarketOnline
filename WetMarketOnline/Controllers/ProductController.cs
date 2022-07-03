using EWM.HelperClass;
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
            //List<MstProduct> pList = MstProduct.GetAllCompleteProductData("Active");

            MstCategory cat1 = new MstCategory()
            {
                CatLevel = 1
            };
            List<MstCategory> catList = cat1.SelectMstCategory("All");
            ViewData["MaxCat"] = MstCategory.GetMaxCat();
            ViewData["CatList"] = catList;
            TempData["HomeFilter"] = section;
            return View();
        }

        public ActionResult ProductCard_Partial(string selectedCategories = "")
        {
            List<MstProduct> productList = MstProduct.GetAllCompleteProductData("Active");
            string homeFilter = (TempData["HomeFilter"] == null) ? "" : TempData["HomeFilter"].ToString();

            if (!string.IsNullOrEmpty(homeFilter))
            {
                MstCategory cat = new MstCategory()
                {
                    CatLevel = 1,
                    CategoryDesc = TempData["HomeFilter"].ToString()
                };
                List<MstCategory> catList = cat.SelectMstCategory("All");

                if (catList.Count > 0)
                {
                    string[] catArr = { catList[0].CategoryId };
                    productList = MstProduct.GetProductByCategory(catArr);
                }

            }

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
            if (string.IsNullOrEmpty(productId))
            {
                return RedirectToAction("ProductAll");
            }

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

        //? AJAX - Add items to shopping cart
        public ActionResult AddToShoppingCart(string productId, string quantity)
        {
            // get item id > update quantity
            // update nav header

            // Validating that customer is logged in
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            TxnShoppingCart cartItem = cart.GetCartItems().Find(l => l.ProductId == productId);

            if (cartItem == null) // if item is not in cart
            {
                cartItem = new TxnShoppingCart();
                cartItem.CustomerId = user.CustomerId;
                cartItem.ProductId = productId;
                cartItem.Quantity = int.Parse(quantity);
                cartItem.CreateTxnShoppingCartItem(user.Username);

            }
            else //if item is already in cart
            {
                cartItem.Quantity = cartItem.Quantity + int.Parse(quantity);
                cartItem.UpdateTxnShoppingCartItem();
            }

            Session["ShoppingCart"] = cart.RetrieveCartItemsFromDb().Count;

            return Json("Ok");
        }

        //end class
    }
}