using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class ProductController : Controller
    {
        //? Verify Customer Login status
        public ActionResult CheckCustomerLogin()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            return Json("Ok");
        }


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

            if (GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer"))
            {
                MstCustomer user = (MstCustomer)Session["Account"];

                // Checking if customer has purchased the product
                bool hasPurchased = TxnOrderHdr.CheckCustomerProductOrders(user.CustomerId, productId);
                ViewData["Purchased"] = hasPurchased;

                // Checking if the customer has reviewed the product
                MstProductReview review = new MstProductReview();
                review.CustomerId = user.CustomerId;
                review.ProductId = productId;

                int hasReview = review.CheckMstProductReview();

                if (hasReview > 0)
                {
                    ViewData["Reviewed"] = true;
                }
                else
                {
                    ViewData["Reviewed"] = false;
                }
            }

            int productReviews = MstProductReview.GetProductReviewsAll(productId).Count;
            ViewData["ReviewCount"] = productReviews;
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

        public ActionResult ProductReview_Partial(string productId, string readonlyReview = "")
        {
            MstCustomer member = new MstCustomer();
            member.CustomerId = "-1";
            if (GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer"))
            {
                member = (MstCustomer)Session["Account"];
            }

            MstProductReview customerReview = MstProductReview.GetCustomerReview(productId, member.CustomerId);
            ViewData["ProductId"] = productId;
            ViewData["Readonly"] = readonlyReview;
            return PartialView(customerReview);
        }

        public ActionResult ProductReviewAll_PopupPartial(string productId)
        {
            MstCustomer member = new MstCustomer();
            if (GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer"))
            {
                member = (MstCustomer)Session["Account"];
            }
            List<MstProductReview> reviewList = MstProductReview.GetProductReviewsAll(productId);

            return PartialView(reviewList);
        }

        public ActionResult ProductReviewCustomer_Card(string productId, string displayLocation = "")
        {
            MstCustomer member = new MstCustomer();
            if (GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer"))
            {
                member = (MstCustomer)Session["Account"];
            }

            MstProductReview customerReview = MstProductReview.GetCustomerReview(productId, member.CustomerId);
            ViewData["Display"] = displayLocation;
            return PartialView("ProductReviewCard_Partial", customerReview);
        }

        public ActionResult ProductReviewSummary_Partial(string productId, decimal rating = 0)
        {
            System.Data.DataTable dt = MstProductReview.CountProductRatingSummary(productId);
            Dictionary<int, int> reviewSummary = new Dictionary<int, int>();


            int rowIndex = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (rowIndex < dt.Rows.Count)
                    {

                        decimal rowValue = Decimal.Parse(dt.Rows[rowIndex][0].ToString());

                        if ((int)rowValue == i)
                        {
                            reviewSummary.Add(i, int.Parse(dt.Rows[rowIndex][1].ToString()));
                            rowIndex++;
                        }
                        else
                        {
                            reviewSummary.Add(i, 0);
                        }
                    }
                    else
                    {
                        reviewSummary.Add(i, 0);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    reviewSummary.Add(i, 0);
                }
            }


            int productReviews = MstProductReview.GetProductReviewsAll(productId).Count;
            ViewData["ProductRating"] = rating;
            ViewData["ReviewCount"] = productReviews;
            return PartialView(reviewSummary);
        }

        //? Create/Update Product Review
        [HttpPost]
        public ActionResult SubmitProductReview(MstProductReview formData, HttpPostedFileBase[] productReviewImage, FormCollection collection = null)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer member = (MstCustomer)Session["Account"];

            string productId = collection.Get("productId");
            if (string.IsNullOrEmpty(collection.Get("reviewId"))) // Create Product Review
            {
                MstProductReview review = new MstProductReview();
                review.ReviewId = Guid.NewGuid().ToString();
                review.Review = collection.Get("productReviewText");
                review.ProductId = productId;
                review.RatingValue = decimal.Parse(collection.Get("rating"));
                review.CustomerId = member.CustomerId;

                review.CreateMstProductReview(member.Username);

                // Upload new product review images
                foreach (var item in productReviewImage)
                {
                    UploadProductReviewImage(item, review.ReviewId, member.Username);
                }
            }
            else // Update Product Review
            {

                MstProductReview review = MstProductReview.GetMstProductReview(collection.Get("reviewId"));
                review.Review = collection.Get("productReviewText");
                review.RatingValue = decimal.Parse(collection.Get("rating"));

                review.UpdateMstProductReview(member.Username);

                // Delete selected product review images
                string[] imageDeleteList = collection.Get("deleteReviewImage").Split(';');
                foreach (var item in review.GetReviewImages())
                {
                    if (imageDeleteList.Contains(item.ReviewImageId))
                    {
                        item.DeleteMstProductReviewImage();
                    }
                }

                // Upload new product review images
                foreach (var item in productReviewImage)
                {
                    UploadProductReviewImage(item, review.ReviewId, member.Username);
                }

            }

            // Update Product Rating
            decimal rating = MstProductReview.CalculateProductRating(productId);
            MstProduct product = MstProduct.GetMstProduct(productId);
            product.Rating = rating;
            product.UpdateMstProduct();

            return RedirectToAction("ProductSingle", new { productId });
        }

        // Upload Review Image and save to database
        private void UploadProductReviewImage(HttpPostedFileBase img, string reviewId, string username = "")
        {
            string fileExtension = "";
            string filePath = "";

            //Get absolute Upload path 
            string UploadPath = Server.MapPath(GeneralBLL.FileDirectory);

            // File Path Configuration
            if (img != null)
            {
                fileExtension = Path.GetExtension(img.FileName);

                //Add Current Date To Attached File Name : Filename_20220226.png
                string completeFileName = img.FileName.Trim() + "_" + DateTime.Now.ToString("yyyyMMdd") + fileExtension;

                //Create relative path to store in server.  
                filePath = typeof(MstProductReviewImages).Name + "/" + completeFileName;

                GeneralBLL.MapFilePath(string.Concat(UploadPath, typeof(MstProductReviewImages).Name));

                //To copy and save file into server.  
                img.SaveAs(String.Concat(UploadPath, filePath));


                // Create record in database
                MstProductReviewImages reviewImg = new MstProductReviewImages();
                reviewImg.ReviewId = reviewId;
                reviewImg.Filename = img.FileName;
                reviewImg.ImageDesc = img.FileName;
                reviewImg.FileLocation = filePath;
                reviewImg.ExtensionType = fileExtension;
                reviewImg.CreateMstProductReviewImage(username);
            }

        }

        //end class
    }
}