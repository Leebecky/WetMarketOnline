using System;

namespace EWM.Models
{
    public class MstProductImage
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstProductImage));

        string ProductImageId { get; set; }
        string ProductId { get; set; }
        string Filename { get; set; }
        int ImageOrder{ get; set; }
        string ImageDesc { get; set; }
        string FileLocation { get; set; }
        string ExtensionType { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstProductImage() { }

    


    }
}