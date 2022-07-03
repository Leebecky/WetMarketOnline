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
        public ActionResult ProductDetails(string id, string mode, int productLevel = 1)
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
                return View("MstProduct_Details", new MstProduct() { ProductId = Guid.NewGuid().ToString() });
            }
            else
            {
                MstProduct productData = MstProduct.GetCompleteProductData(id);

                //if (productLevel > 1)
                //{
                //    MstProduct parent = MstProduct.GetMstProduct(productData.ParentCatId);
                //    productData.ParentCatId = parent.ProductCode;
                //}
                return View("MstProduct_Details", productData);
            }
        }

        [HttpPost]
        public ActionResult UpdateMstProduct(MstProduct formData, string mode, FormCollection MstProductForm = null, int maxCat = 1)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant member = (MstMerchant)Session["Account"];
            List<MstProductCategory> formCategoryList = new List<MstProductCategory>();
            int rowsAffected = -1;

            for (int i = 1; i <= maxCat; i++)
            {
                string category = MstProductForm.Get("Cat" + i);
                if (!string.IsNullOrEmpty(category))
                {
                    MstProductCategory cat = new MstProductCategory();
                    cat.ProductId = formData.ProductId;
                    cat.CatId = category;
                    formCategoryList.Add(cat);
                }
            }

            if (mode == "New")
            {
                rowsAffected = formData.CreateMstProduct(member.Username);

                //Add product category
                foreach (var item in formCategoryList)
                {
                    item.CreateMstProductCategory(member.Username);
                }

            }
            else if (mode == "Edit")
            {
                // Delete existing product category data
                MstProductCategory existingCat = new MstProductCategory();
                existingCat.ProductId = formData.ProductId;
                List<MstProductCategory> pCatList = existingCat.SelectMstProductCategory("All");
                foreach (var item in pCatList)
                {
                    item.DeleteMstProductCategory();
                }


                // Update Mst Product
                MstProduct oriData = MstProduct.GetMstProduct(formData.ProductId);
                oriData.ProductCode = formData.ProductCode;
                oriData.ProductName = formData.ProductName;
                oriData.ProductDesc = formData.ProductDesc;
                oriData.Price = formData.Price;
                oriData.Quantity = formData.Quantity;
                oriData.Status = formData.Status;

                rowsAffected = oriData.UpdateMstProduct(member.Username);

                //Add product category
                foreach (var item in formCategoryList)
                {
                    item.CreateMstProductCategory(member.Username);
                }
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
        public ActionResult DeleteProduct(string id)
        {
            MstProduct deleteCat = MstProduct.GetCompleteProductData(id);

            int rowsAffected = deleteCat.DeleteMstProduct();

            foreach (var item in deleteCat.GetCatList())
            {
                item.DeleteMstCategory();
            }

            foreach (var item in deleteCat.GetImageList())
            {
                item.DeleteMstProductImage();
            }

            if (rowsAffected != 1)
            {
                TempData["Error"] = "Failed to delete the product. Please try again.";
            }
            return RedirectToAction("ManageProduct");
        }

        public ActionResult ProductImage_Partial(string productId, List<MstProductImage> imgList = null, string pageMode = "")
        {


            if (imgList == null || imgList.Count == 0)
            {
                MstProductImage search = new MstProductImage() { ProductId = productId };
                imgList = search.SelectMstProductImage("All");
            }

            ViewData["PageMode"] = pageMode;
            return PartialView(imgList);
        }

        //? AJAX call for Parent Cat dropdown
        public ActionResult GetParentCat_Cb(int catLevel = 1, string parentId = "", string pageMode = "", string selectedData = "")
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

                if (pageMode == "New" && i == 0 && catLevel == 1)
                {
                    selectionItem.Selected = true;
                }

                if (selectedData == item.CategoryId)
                {
                    selectionItem.Selected = true;
                }
                selectionList.Add(selectionItem);
            }

            return Json(selectionList);
        }

        public ActionResult ProductImage_PopupPartial(string productId = "", string productImageId = "")
        {
            MstProductImage img = (string.IsNullOrEmpty(productImageId)) ? new MstProductImage() { ProductId = productId } : MstProductImage.GetMstProductImage(productImageId);            
            string jsonImg = Newtonsoft.Json.JsonConvert.SerializeObject(img);
            return Content(jsonImg);
        }

        [HttpPost]
        public ActionResult SaveMstProductImage(MstProductImage popupImage = null, HttpPostedFileBase ProductImageUpload = null, FormCollection MstProductImageForm = null)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant member = (MstMerchant)Session["Account"];
            string fileExtension = "";
            string filePath = "";
            int rowsAffected = -1;
            string mode = MstProductImageForm.Get("Mode_Img");

            //Get absolute Upload path 
            string UploadPath = Server.MapPath(GeneralBLL.FileDirectory);

            popupImage.ProductId = MstProductImageForm.Get("ProductId_Img");
            popupImage.Status = MstProductImageForm.Get("Status_Img");

            // File Path Configuration
            if (ProductImageUpload != null)
            {
                string fileName = MstProductImageForm.Get("Filename");
                fileExtension = System.IO.Path.GetExtension(ProductImageUpload.FileName);

                //Add Current Date To Attached File Name : Filename_20220226.png
                string completeFileName = fileName.Trim() + "_" + DateTime.Now.ToString("yyyyMMdd") + fileExtension;

                //Create relative path to store in server.  
                filePath = typeof(MstProductImage).Name + "/" + popupImage.ProductId + "/" + completeFileName;

                GeneralBLL.MapFilePath(string.Concat(UploadPath, typeof(MstProductImage).Name, "/", popupImage.ProductId));

                //To copy and save file into server.  
                ProductImageUpload.SaveAs(String.Concat(UploadPath, filePath));


                // Set to form data values
                popupImage.ExtensionType = fileExtension;
                popupImage.FileLocation = filePath;
            }

            if (mode == "Edit")
            {
                MstProductImage oriData = MstProductImage.GetMstProductImage(popupImage.ProductImageId);
                oriData.Filename = popupImage.Filename;
                oriData.Status = popupImage.Status;
                oriData.ImageDesc = popupImage.ImageDesc;
                oriData.ImageOrder = popupImage.ImageOrder;

                oriData.FileLocation = popupImage.FileLocation;
                oriData.ExtensionType = popupImage.ExtensionType;

                if (ProductImageUpload == null) //if no file was uploaded
                {
                    string newFilePath = popupImage.FileLocation.Replace(oriData.GetOriFileName(), popupImage.Filename);
                    System.IO.File.Move(string.Concat(UploadPath, popupImage.FileLocation), string.Concat(UploadPath, newFilePath));

                    oriData.FileLocation = newFilePath;
                }
                rowsAffected = oriData.UpdateMstProductImage(member.Username);
            }
            else if (mode == "New")
            {
                rowsAffected = popupImage.CreateMstProductImage(member.Username);
            }

            if (rowsAffected == 1)
            {
                return Content("OK");
                //return View("ProductImage_Partial", imgList);
            }
            else
            {
                ViewBag.Error = "Error processing request. Please try again";
                return Content("ERROR");
            }
            //return View("MstProduct_Details", product);
        }

        [HttpPost]
        public ActionResult DeleteProductImage(string id)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstProductImage delItem = MstProductImage.GetMstProductImage(id);
            int rowsAffected = delItem.DeleteMstProductImage();

            string UploadPath = Server.MapPath(GeneralBLL.FileDirectory);
            System.IO.File.Delete(String.Concat(UploadPath, delItem.FileLocation));
            if (rowsAffected != 1)
            {
                return Content("Failed to delete the product. Please try again.");
            }
            return Content("OK");
        }

        //? Order List page
        public ActionResult OrderList_Merchant()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            return View();
        }

        public ActionResult OrderCard_MerchantPartial(string status = "")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant member = (MstMerchant)Session["Account"];

            List<TxnOrderHdr> orderList = TxnOrderHdr.GetMerchantOrders(member.MerchantId, status);            
            orderList = orderList.OrderByDescending(l => l.UpdatedDate).ToList();
            return PartialView(orderList);
        }


        //? Order Tracking page
        public ActionResult OrderTracking_Merchant(string orderHdrId)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Merchant")) { return RedirectToAction("Login", "Account"); }
            MstMerchant member = (MstMerchant)Session["Account"];

            TxnOrderHdr hdr = TxnOrderHdr.GetTxnOrderHdr(orderHdrId);
            hdr.SetOrderDetails(TxnOrderDtl.GetCompleteMerchantOrderDetails(orderHdrId, member.MerchantId));

            return View(hdr);
        }

        //end class
    }
}