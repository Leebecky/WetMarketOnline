using System;

namespace EWM.Models
{
    public class MstProductReview
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstProductReview));

        string ReviewId { get; set; }
        string CustomerId { get; set; }
        string RatingValue { get; set; }
        string Review { get; set; }
        string ProductId { get; set; }
        bool CustomerPurchase { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstProductReview() { }

    


    }
}