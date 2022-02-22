using System;

namespace EWM.Models
{
    public class TxnOrderDtl
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(TxnOrderDtl).AssemblyQualifiedName;

        public string OrderDtlId { get; set; }
        public string OrderHdrId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
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

        // Default Constructor
        public TxnOrderDtl() { }

    


    }
}