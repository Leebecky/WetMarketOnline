using System;

namespace EWM.Models
{
    public class MstProductReviewImage
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstProductReviewImage).AssemblyQualifiedName;

        public string ReviewImageId { get; set; }
        public string ReviewId { get; set; }
        public string Filename { get; set; }        
        public string FileLocation { get; set; }
        public string ImageDesc { get; set; }
        public string ExtensionType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriReviewImageId { get; set; }
        private string OriReviewId { get; set; }
        private string OriFilename { get; set; }
        private string OriFileLocation { get; set; }
        private string OriImageDesc { get; set; }
        private string OriExtensionType { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProductReviewImage() { }

    


    }
}