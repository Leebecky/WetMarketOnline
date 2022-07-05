using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace EWM.Models
{
    public class MstProductReviewImages
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProductReviewImages).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstProductReviewImages>).AssemblyQualifiedName;

        public string ReviewImageId { get; set; }
        public string ReviewId { get; set; }
        public string Filename { get; set; }
        public string FileLocation { get; set; }
        public string ImageDesc { get; set; }
        public string ExtensionType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriReviewImageId { get; set; }
        private string OriReviewId { get; set; }
        private string OriFilename { get; set; }
        private string OriFileLocation { get; set; }
        private string OriImageDesc { get; set; }
        private string OriExtensionType { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProductReviewImages() { }

        // Constructor - Retrieve from Db based on PK
        public static MstProductReviewImages GetMstProductReviewImage(string reviewImageId)
        {
            MstProductReviewImages review = new MstProductReviewImages();
            review.ReviewImageId = reviewImageId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, review, filterType: "All");
            List<MstProductReviewImages> reviewList = (List<MstProductReviewImages>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (reviewList.Count == 1)
            {
                return reviewList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstProductReviewImage(string userName = "")
        {
            this.ReviewImageId = Guid.NewGuid().ToString();
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
        public int UpdateMstProductReviewImage(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstProductReviewImage()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Select records
        public List<MstProductReviewImages> SelectMstProductReviewImage(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstProductReviewImages> data = (List<MstProductReviewImages>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstProductReviewImage()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        #endregion


    }
}