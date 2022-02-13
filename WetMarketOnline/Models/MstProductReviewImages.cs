using System;

namespace EWM.Models
{
    public class MstProductReviewImage
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstProductReviewImage));

        string ReviewImageId { get; set; }
        string ReviewId { get; set; }
        string Filename { get; set; }        
        string FileLocation { get; set; }
        string ImageDesc { get; set; }
        string ExtensionType { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstProductReviewImage() { }

    


    }
}