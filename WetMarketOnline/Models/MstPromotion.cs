using System;

namespace EWM.Models
{
    public class MstPromotion
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstPromotion).AssemblyQualifiedName;

        public string PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionDesc { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriPromotionId { get; set; }
        private string OriPromotionCode { get; set; }
        private string OriPromotionDesc { get; set; }
        private decimal OriAmount { get; set; }
        private DateTime OriStartDate { get; set; }
        private DateTime OriEndDate { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstPromotion() { }

    


    }
}