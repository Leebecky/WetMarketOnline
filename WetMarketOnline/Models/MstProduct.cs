using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstProduct
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProduct).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstProduct>).AssemblyQualifiedName;

        public string ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Display(Name = "Description")]
        public string ProductDesc { get; set; }
        [Display(Name = "Merchant")]
        public string MerchantId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rating { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Last Updated")]
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        // Additional Properties
        private List<MstCategory> CatList { get; set; }
        private List<MstProductImage> ImageList { get; set; }
        private List<MstProductReview> ReviewList { get; set; }
        private List<MstProductReviewImage> ReviewImageList { get; set; }

        // Original Data
        private string OriProductId { get; set; }
        private string OriProductName { get; set; }
        private string OriProductCode { get; set; }
        private string OriProductDesc { get; set; }
        private string OriMerchantId { get; set; }
        private decimal OriPrice { get; set; }
        private int OriQuantity { get; set; }
        private decimal OriRating { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProduct() { }

        // Constructor - Retrieve from Db based on PK
        public static MstProduct GetMstProduct(string productId)
        {
            MstProduct merchant = new MstProduct();
            merchant.ProductId = productId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, merchant, filterType: "All");
            List<MstProduct> productList = (List<MstProduct>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (productList.Count == 1)
            {
                return productList[0];
            }
            return null;
        }

        #region Getters & Setters

        public List<MstCategory> GetCatList()
        {
            return CatList;
        }

        public List<MstProductImage> GetImageList()
        {
            return ImageList;
        }

        public List<MstProductReview> GetReviewList()
        {
            return ReviewList;
        }

        public List<MstProductReviewImage> GetReviewImageList()
        {
            return ReviewImageList;
        }

        #endregion

        #region Methods

        //? Insert new record
        public int CreateMstProduct(string userName = "")
        {
            this.ProductId = (String.IsNullOrEmpty(this.ProductId)) ?  Guid.NewGuid().ToString()  : this.ProductId;
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
            this.CreatedBy = userName;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Insert");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Update existing record
        public int UpdateMstProduct(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstProduct()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Insert new record
        public List<MstProduct> SelectMstProduct(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstProduct> data = (List<MstProduct>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }


        //? Select all records from database
        public static List<MstProduct> SelectMstProduct_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstProduct> data = (List<MstProduct>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstProduct>();
            }
            return data;
        }

        //? Assembles a complete set of data for MstProduct
        public static MstProduct GetCompleteProductData(string productId)
        {
            MstProduct product = GetMstProduct(productId);
            product.CatList = product.GetMstProductCategoryData();
            product.ImageList = product.GetMstProductImageData();
            return product;
        }

        //? Retrieves the Product Category Data
        public List<MstCategory> GetMstProductCategoryData()
        {
            List<MstCategory> catList = new List<MstCategory>();
            try
            {
                MstProductCategory productCat = new MstProductCategory();
                productCat.ProductId = this.ProductId;
                productCat.Status = "Active";

                List<MstProductCategory> productCatList = productCat.SelectMstProductCategory("All");

                foreach (var item in productCatList)
                {
                    MstCategory cat = MstCategory.GetMstCategory(item.CatId);
                    catList.Add(cat);
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetMstProductCategoryData: " + ex);
            }
            return catList;
        }

        //? Retrieves the Product Category Data
        public List<MstProductImage> GetMstProductImageData()
        {
            List<MstProductImage> productImgList = new List<MstProductImage>();
            try
            {
                MstProductImage productImg = new MstProductImage
                {
                    ProductId = this.ProductId
                };
                productImgList = productImg.SelectMstProductImage("All");
            }
            catch (Exception ex)
            {
                Log.Error("GetMstProductImageData: " + ex);
            }
            return productImgList;
        }


        #endregion        
    }
}