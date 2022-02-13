using System;

namespace EWM.Models
{
    public class TxnOrderHdr
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(TxnOrderHdr));

        string OrderHdrId { get; set; }
        string CustomerId { get; set; }
        DateTime OrderDate { get; set; }
        string PromotionId { get; set; }
        decimal ShippingFee { get; set; }
        decimal Discount { get; set; }
        decimal TotalPrice { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public TxnOrderHdr() { }

    


    }
}