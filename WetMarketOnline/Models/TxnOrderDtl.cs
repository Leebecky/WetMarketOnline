using System;

namespace EWM.Models
{
    public class TxnOrderDtl
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(TxnOrderDtl));

        string OrderDtlId { get; set; }
        string OrderHdrId { get; set; }
        string ProductId { get; set; }
        int Quantity { get; set; }
        decimal Price { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public TxnOrderDtl() { }

    


    }
}