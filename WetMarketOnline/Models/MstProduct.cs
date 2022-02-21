using System;

namespace EWM.Models
{
    public class MstProduct
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProduct).AssemblyQualifiedName;

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string MerchantId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }


        private string OriProductId { get; set; }
        private string OriProductName { get; set; }
        private string OriProductDesc { get; set; }
        private string OriMerchantId { get; set; }
        private decimal OriPrice { get; set; }
        private int OriQuantity { get; set; }
        private decimal OriRating { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProduct() { }

    


    }
}