using System;

namespace EWM.Models
{
    public class MstProductReview
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProductReview).AssemblyQualifiedName;

        public string ReviewId { get; set; }
        public string CustomerId { get; set; }
        public string RatingValue { get; set; }
        public string Review { get; set; }
        public string ProductId { get; set; }
        public bool CustomerPurchase { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriReviewId { get; set; }
        private string OriCustomerId { get; set; }
        private string OriRatingValue { get; set; }
        private string OriReview { get; set; }
        private string OriProductId { get; set; }
        private bool OriCustomerPurchase { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProductReview() { }

    


    }
}