using EWM.HelperClass;
using System;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstProduct
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProduct).AssemblyQualifiedName;

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string MerchantId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }


        private string OriProductId { get; set; }
        private string OriProductName { get; set; }
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
            merchant = (MstProduct)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName);

            return merchant;
        }

        #region Methods

        //? Insert new record
        public int CreateMstProduct(string userName = "")
        {
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
        public MstProduct SelectMstProduct(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            MstProduct data = (MstProduct)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName);

            return data;
        }
        #endregion


    }
}