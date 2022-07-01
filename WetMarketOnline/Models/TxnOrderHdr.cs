using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class TxnOrderHdr
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(TxnOrderHdr).AssemblyQualifiedName;
        public static string ListName = typeof(List<TxnOrderHdr>).AssemblyQualifiedName;

        public string OrderHdrId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string PromotionId { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriOrderHdrId { get; set; }
        private string OriCustomerId { get; set; }
        private DateTime OriOrderDate { get; set; }
        private string OriPromotionId { get; set; }
        private decimal OriShippingFee { get; set; }
        private decimal OriDiscount { get; set; }
        private decimal OriTotalPrice { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public TxnOrderHdr() { }


        // Constructor - Retrieve from Db based on PK
        public static TxnOrderHdr GetTxnOrderHdr(string orderHdrId)
        {
            TxnOrderHdr orderHdr = new TxnOrderHdr();
            orderHdr.OrderHdrId = orderHdrId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, orderHdr, filterType: "All");
            List<TxnOrderHdr> orderHdrList = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (orderHdrList.Count == 1)
            {
                return orderHdrList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateTxnOrderHdr(string userName = "")
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
        public int UpdateTxnOrderHdr(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteTxnOrderHdr()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Find relevant record
        public List<TxnOrderHdr> SelectTxnOrderHdr(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<TxnOrderHdr> data = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }
        #endregion
    }
}