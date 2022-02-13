using System;

namespace EWM.Models
{
    public class MstPromotion
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstPromotion));

        string PromotionId { get; set; }
        string PromotionCode { get; set; }
        string PromotionDesc { get; set; }
        decimal Amount { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstPromotion() { }

    


    }
}