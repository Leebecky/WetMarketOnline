using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstProductImage
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProductImage).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstProductImage>).AssemblyQualifiedName;

        public string ProductImageId { get; set; }
        public string ProductId { get; set; }
        public string Filename { get; set; }
        public int? ImageOrder{ get; set; }
        public string ImageDesc { get; set; }
        public string FileLocation { get; set; }
        public string ExtensionType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriProductImageId { get; set; }
        private string OriProductId { get; set; }
        private string OriFilename { get; set; }
        private int? OriImageOrder { get; set; }
        private string OriImageDesc { get; set; }
        private string OriFileLocation { get; set; }
        private string OriExtensionType { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProductImage() { }

        // Constructor - Retrieve from Db based on PK
        public static MstProductImage GetMstProductImage(string imgId)
        {
            MstProductImage img = new MstProductImage();
            img.ProductImageId = imgId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, img, filterType: "All");
            List<MstProductImage> imgList = (List<MstProductImage>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (imgList.Count == 1)
            {
                return imgList[0];
            }
            return null;
        }

        #region Getters & Setters

        public string GetOriFileName()
        {
            return OriFilename;
        }

        #endregion

        #region Methods

        //? Insert new record
        public int CreateMstProductImage(string userName = "")
        {
            this.ProductImageId = Guid.NewGuid().ToString();
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
        public int UpdateMstProductImage(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstProductImage()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Find Data from table
        public List<MstProductImage> SelectMstProductImage(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstProductImage> data = (List<MstProductImage>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstProductImage()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        //? Select all records from database
        public static List<MstProductImage> SelectMstProductImage_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstProductImage> data = (List<MstProductImage>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstProductImage>();
            }
            return data;
        }


        #endregion


    }
}