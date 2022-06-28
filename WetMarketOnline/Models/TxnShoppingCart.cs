using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class TxnShoppingCart
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(TxnShoppingCart).AssemblyQualifiedName;
        public static string ListName = typeof(List<TxnShoppingCart>).AssemblyQualifiedName;


        public string CartId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriCartId { get; set; }
        private string OriCustomerId { get; set; }
        private string OriProductId { get; set; }
        private int OriQuantity { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Additional Properties
        private List<TxnShoppingCart> CartItems { get; set; }
        private MstProduct ProductItem { get; set; }

        // Default Constructor
        public TxnShoppingCart() { }

        #region Getters & Setters


        public List<TxnShoppingCart> GetCartItems()
        {
            return CartItems;
        }
        public List<TxnShoppingCart> RetrieveCartItemsFromDb()
        {
            CartItems = this.SelectTxnShoppingCart("All");

            foreach (var item in CartItems)
            {
                item.SetProductItem(MstProduct.GetCompleteProductData(item.ProductId, "Active"));
            }

            return CartItems;
        }

        public void SetCartItems(List<TxnShoppingCart> cartItems)
        {
            CartItems = cartItems;
        }

        public MstProduct GetProductItem()
        {
            return ProductItem;
        }

        public void SetProductItem(MstProduct item)
        {
            ProductItem = item;
        }
        #endregion

        #region Methods
        // Constructor - Retrieve from Db based on PK
        public static TxnShoppingCart GetTxnShoppingCartItems(string customerId)
        {
            TxnShoppingCart cart = new TxnShoppingCart();
            cart.CustomerId = customerId;

            // Get Cart Items
            cart.RetrieveCartItemsFromDb();
            //cart.SetCartItems(cart.SelectTxnShoppingCart("All"));

            //foreach (var item in cart.GetCartItems())
            //{
            //    item.SetProductItem(MstProduct.GetCompleteProductData(item.ProductId, "Active"));
            //}

            return cart;
        }

        //? Insert new record
        public int CreateTxnShoppingCartItem(string userName = "")
        {
            this.CartId = Guid.NewGuid().ToString();
            this.Status = "Active";
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
            this.CreatedBy = userName;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Insert");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            if (rowsAffected > 0)
            {
                MstProduct product = MstProduct.GetCompleteProductData(this.ProductId);
                product.Quantity = product.Quantity - this.Quantity;
                product.UpdateMstProduct();
            }

            return rowsAffected;
        }

        //? Update existing record
        public int UpdateTxnShoppingCartItem(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            if (rowsAffected > 0)
            {
                MstProduct product = MstProduct.GetCompleteProductData(this.ProductId);

                // Updating the Product Quantity
                if (this.Quantity > this.OriQuantity) // Quantity was added
                {
                    product.Quantity = product.Quantity - (this.Quantity - this.OriQuantity);
                }

                if (this.Quantity < this.OriQuantity) // Quantity was reduced
                {
                    product.Quantity = product.Quantity + (this.OriQuantity - this.Quantity);
                }

                product.UpdateMstProduct();
            }

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteTxnShoppingCartItem()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            if (rowsAffected > 0)
            {
                MstProduct product = MstProduct.GetCompleteProductData(this.ProductId);
                product.Quantity = product.Quantity + this.Quantity;
                product.UpdateMstProduct();
            }

            return rowsAffected;
        }

        //? Find Data from table
        public List<TxnShoppingCart> SelectTxnShoppingCart(string filterType = "Column")
        {
            //SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            string sql = "Select * from txn_shopping_cart where customer_id = @customerId";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@customerId", this.CustomerId);
            List<TxnShoppingCart> data = (List<TxnShoppingCart>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckTxnShoppingCart()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }


        #endregion
    }
}