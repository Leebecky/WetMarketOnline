using System;

namespace EWM.Models
{
    public class TxnShoppingCart
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(TxnShoppingCart));

        string CartId { get; set; }
        string CustomerId { get; set; }
        string ProductId { get; set; }
        int Quantity { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public TxnShoppingCart() { }

    


    }
}