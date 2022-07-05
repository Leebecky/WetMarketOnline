using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstProductReview
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProductReview).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstProductReview>).AssemblyQualifiedName;

        public string ReviewId { get; set; }
        public string CustomerId { get; set; }
        public decimal? RatingValue { get; set; }
        public string Review { get; set; }
        public string ProductId { get; set; }
        public bool CustomerPurchase { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriReviewId { get; set; }
        private string OriCustomerId { get; set; }
        private decimal OriRatingValue { get; set; }
        private string OriReview { get; set; }
        private string OriProductId { get; set; }
        private bool OriCustomerPurchase { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Additional Values
        private List<MstProductReviewImages> ReviewImages { get; set; }
        private string CustomerName { get; set; }

        #region Getters & Setters
        public List<MstProductReviewImages> GetReviewImages()
        {
            return ReviewImages;
        }

        public void SetReviewImages()
        {
            if (!string.IsNullOrEmpty(this.ReviewId))
            {
                MstProductReviewImages img = new MstProductReviewImages
                {
                    ReviewId = this.ReviewId
                };

                List<MstProductReviewImages> reviewImg = img.SelectMstProductReviewImage("All");
                if (reviewImg != null)
                {
                    ReviewImages = reviewImg;
                } else
                {
                    ReviewImages = new List<MstProductReviewImages>();
                }
            }
            else
            {
                ReviewImages = new List<MstProductReviewImages>();
            }
        }

        public string GetCustomerName()
        {
            return CustomerName;
        }

        public void SetCustomerName()
        {
            if (!string.IsNullOrEmpty(this.CustomerId))
            {
                MstCustomer customer = MstCustomer.GetMstCustomer(this.CustomerId);
                if (customer != null)
                {
                    CustomerName = customer.Name;
                }
            }
            else
            {
                CustomerName = "";
            }
        }
        #endregion

        // Default Constructor
        public MstProductReview() { }

        // Constructor - Retrieve from Db based on PK
        public static MstProductReview GetMstProductReview(string reviewId)
        {
            MstProductReview review = new MstProductReview();
            review.ReviewId = reviewId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, review, filterType: "All");
            List<MstProductReview> reviewList = (List<MstProductReview>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (reviewList.Count == 1)
            {
                reviewList[0].SetCustomerName();
                reviewList[0].SetReviewImages();
                return reviewList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstProductReview(string userName = "")
        {
        
            this.Status = "Active";
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
            this.CreatedBy = userName;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Insert");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Update existing record
        public int UpdateMstProductReview(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstProductReview()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Insert new record
        public List<MstProductReview> SelectMstProductReview(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstProductReview> data = (List<MstProductReview>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstProductReview()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }
        
        //? Retrieve customer product review
        public static MstProductReview GetCustomerReview(string productId, string customerId)
        {
            MstProductReview review = new MstProductReview();
            review.CustomerId = customerId;
            review.ProductId = productId;            

            List<MstProductReview> reviewList = review.SelectMstProductReview("All");

            if (reviewList.Count == 1)
            {
                reviewList[0].SetCustomerName();
                reviewList[0].SetReviewImages();
                return reviewList[0];
            }
            return new MstProductReview();
        }

        //? Retrieve product review
        public static List<MstProductReview> GetProductReviewsAll(string productId)
        {
            MstProductReview review = new MstProductReview();
            review.ProductId = productId;

            List<MstProductReview> reviewList = review.SelectMstProductReview("All");

            foreach (var item in reviewList)
            {
                item.SetReviewImages();
                item.SetCustomerName();
            }

            return reviewList;
        }

        // ? Calculate Product Rating
        public static decimal CalculateProductRating(string productId)
        {
            decimal rating = 0;

            string sql = "Select IsNull(AVG(rating_value), 0) from mst_product_review where product_id = @productId";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@productId", productId);

            DataTable dt = DatabaseManager.ExecuteQueryCommand_Datatable(cmd);
            if (dt.Rows.Count > 0)
            {
                rating = decimal.Parse(dt.Rows[0][0].ToString());
            }
            return rating;           
        }

        //? Counts Number of Ratings 
        public static DataTable CountProductRatingSummary(string productId)
        {
            string sql = "Select rating_value as 'RatingValue' , IsNull(Count(rating_value) ,0) as 'RatingCount' from mst_product_review where product_id = @productId Group By rating_value, product_id Order By rating_value";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@productId", productId);
            DataTable dt = DatabaseManager.ExecuteQueryCommand_Datatable(cmd);

            return dt;
        }

        #endregion
    }
}