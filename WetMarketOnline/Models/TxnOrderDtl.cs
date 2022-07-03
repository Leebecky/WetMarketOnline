using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class TxnOrderDtl
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(TxnOrderDtl).AssemblyQualifiedName;
        public static string ListName = typeof(List<TxnOrderDtl>).AssemblyQualifiedName;

        public string OrderDtlId { get; set; }
        public string OrderHdrId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriOrderDtlId { get; set; }
        private string OriOrderHdrId { get; set; }
        private string OriProductId { get; set; }
        private int OriQuantity { get; set; }
        private decimal OriPrice { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Additional Variables
        private MstProduct OrderItem { get; set; }

        #region Getters & Setters

        public MstProduct GetOrderItem()
        {
            return OrderItem;
        }

        #endregion

        // Default Constructor
        public TxnOrderDtl() { }

        // Constructor - Retrieve from Db based on PK
        public static TxnOrderDtl GetTxnOrderDtl(string orderDtlId)
        {
            TxnOrderDtl orderDtl = new TxnOrderDtl();
            orderDtl.OrderHdrId = orderDtlId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, orderDtl, filterType: "All");
            List<TxnOrderDtl> orderDtlList = (List<TxnOrderDtl>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (orderDtlList.Count == 1)
            {
                return orderDtlList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateTxnOrderDtl(string userName = "")
        {
            this.OrderDtlId = Guid.NewGuid().ToString();
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
        public int UpdateTxnOrderDtl(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteTxnOrderDtl()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Find relevant record
        public List<TxnOrderDtl> SelectTxnOrderDtl(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<TxnOrderDtl> data = (List<TxnOrderDtl>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Retrieve Order details with Product Data
        public static List<TxnOrderDtl> GetCompleteOrderDetails(string hdrId)
        {
            TxnOrderDtl dtl = new TxnOrderDtl();
            dtl.OrderHdrId = hdrId;
            List<TxnOrderDtl> dtlList = dtl.SelectTxnOrderDtl("All");

            foreach (var item in dtlList)
            {
                item.OrderItem = MstProduct.GetCompleteProductData(item.ProductId, "Active");
            }

            return dtlList;
        }

        //? Retrieve Order details with Product Data based on merchant id
        public static List<TxnOrderDtl> GetCompleteMerchantOrderDetails(string hdrId, string merchantId)
        {
            string sql = "Select d.* from txn_order_dtl d Inner Join mst_product p on p.product_id = d.product_id Where p.merchant_id = @merchantId and d.order_hdr_id = @orderHdrId";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@merchantId", merchantId);
            cmd.Parameters.AddWithValue("@orderHdrId", hdrId);

            List<TxnOrderDtl> dtlList = (List<TxnOrderDtl>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            foreach (var item in dtlList)
            {
                item.OrderItem = MstProduct.GetCompleteProductData(item.ProductId, "Active");
            }

            return dtlList;
        }

        #endregion  
    }
}