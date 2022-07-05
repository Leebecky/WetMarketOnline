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

        // Additional Variables
        private List<TxnOrderDtl> OrderDetails { get; set; }
        private string PromotionCode { get; set; }
        private string CustomerName { get; set; }

        #region Getters & Setters

        public List<TxnOrderDtl> GetOrderDetails()
        {
            return OrderDetails;
        }

        public void SetOrderDetails(List<TxnOrderDtl> orderDtl)
        {
            OrderDetails = orderDtl;
        }

        public string GetPromotionCode()
        {
            return PromotionCode;
        }

        public void SetPromotionCode()
        {
            if (!string.IsNullOrEmpty(this.PromotionId))
            {
                MstPromotion promo = MstPromotion.GetMstPromotion(this.PromotionId);
                if (promo != null)
                {
                    PromotionCode = promo.PromotionCode;
                }
            }
            else
            {
                PromotionCode = "";
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
                orderHdrList[0].SetPromotionCode();
                orderHdrList[0].SetCustomerName();
                orderHdrList[0].OrderDetails = TxnOrderDtl.GetCompleteOrderDetails(orderHdrList[0].OrderHdrId);
                return orderHdrList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateTxnOrderHdr(string userName = "")
        {
            this.Status = "Order Confirmed";
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

        public static List<TxnOrderHdr> GetCustomerOrder(string customerId, string status = "")
        {
            TxnOrderHdr orderHdr = new TxnOrderHdr();
            orderHdr.CustomerId = customerId;

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                orderHdr.Status = status;
            }

            List<TxnOrderHdr> orderHdrList = orderHdr.SelectTxnOrderHdr("All");

            foreach (var order in orderHdrList)
            {
                order.SetPromotionCode();
                order.OrderDetails = TxnOrderDtl.GetCompleteOrderDetails(order.OrderHdrId);
            }
            return orderHdrList;
        }

        public static List<TxnOrderHdr> GetAllOrders(string status = "")
        {
            List<TxnOrderHdr> orderHdrList = new List<TxnOrderHdr>();
            TxnOrderHdr orderHdr = new TxnOrderHdr();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                orderHdr.Status = status;
                orderHdrList = orderHdr.SelectTxnOrderHdr("All");
            }
            else
            {
                SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, orderHdr, "All", "All");
                orderHdrList = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);
            }


            foreach (var order in orderHdrList)
            {
                order.SetPromotionCode();
                order.SetCustomerName();
                order.OrderDetails = TxnOrderDtl.GetCompleteOrderDetails(order.OrderHdrId);
            }
            return orderHdrList;
        }

        public static List<TxnOrderHdr> GetMerchantOrders(string merchantId, string status = "")
        {
            List<TxnOrderHdr> orderHdrList = new List<TxnOrderHdr>();
            TxnOrderHdr orderHdr = new TxnOrderHdr();

            // Looking for orders with products from a specific merchant
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                string sql = "Select h.* from txn_order_hdr h Inner Join txn_order_dtl d on d.order_hdr_id = h.order_hdr_id Inner Join mst_product p on p.product_id = d.product_id Where p.merchant_id = @merchantId and h.status = @status";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@merchantId", merchantId);
                cmd.Parameters.AddWithValue("@status", status);
                orderHdrList = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);
            }
            else
            {
                string sql = "Select h.* from txn_order_hdr h Inner Join txn_order_dtl d on d.order_hdr_id = h.order_hdr_id Inner Join mst_product p on p.product_id = d.product_id Where p.merchant_id = @merchantId";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@merchantId", merchantId);
                orderHdrList = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);
            }

            foreach (var order in orderHdrList)
            {
                order.SetPromotionCode();
                order.SetCustomerName();
                order.OrderDetails = TxnOrderDtl.GetCompleteMerchantOrderDetails(order.OrderHdrId, merchantId);
            }
            return orderHdrList;
        }

        // Check if customer has purchased a product
        public static bool CheckCustomerProductOrders(string customerId, string productId)
        {
            List<TxnOrderHdr> orderHdrList = new List<TxnOrderHdr>();
            TxnOrderHdr orderHdr = new TxnOrderHdr();

            // Looking for orders with products from a specific merchant
            
            string sql = "Select h.* from txn_order_hdr h Inner Join txn_order_dtl d on d.order_hdr_id = h.order_hdr_id Inner Join mst_product p on p.product_id = d.product_id Where h.customer_id = @customerId and p.product_id = @productId";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@customerId", customerId);
            cmd.Parameters.AddWithValue("@productId", productId);
            int rowCount = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);
            //orderHdrList = (List<TxnOrderHdr>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);
            
           if (rowCount > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        #endregion
    }
}