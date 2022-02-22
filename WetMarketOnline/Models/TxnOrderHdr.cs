using System;

namespace EWM.Models
{
    public class TxnOrderHdr
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(TxnOrderHdr).AssemblyQualifiedName;

        public string OrderHdrId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string PromotionId { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
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

    


    }
}