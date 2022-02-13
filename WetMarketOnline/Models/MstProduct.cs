using System;

namespace EWM.Models
{
    public class MstProduct
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstProduct));

        string ProductId { get; set; }
        string ProductName { get; set; }
        string ProductDesc { get; set; }
        string MerchantId { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
        decimal Rating { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstProduct() { }

    


    }
}