using System;

namespace EWM.Models
{
    public class MstHomeSlider
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstHomeSlider));

        string SliderPhotoId { get; set; }
        string FileLocation { get; set; }
        string ImageDesc { get; set; }
        string Filename { get; set; }
        string ExtensionType { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstHomeSlider() { }

    


    }
}