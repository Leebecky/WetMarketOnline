using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstHomeSlider
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstHomeSlider).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstHomeSlider>).AssemblyQualifiedName;
        //public static string FileDirectory = System.Configuration.ConfigurationManager.AppSettings["UploadDirectoryForPhoto"];

        public string SliderPhotoId { get; set; }
        [Display(Name = "File Location")]
        public string FileLocation { get; set; }
        [Display(Name = "Description")]
        public string ImageDesc { get; set; }
        [Display(Name = "File Name")]
        public string Filename { get; set; }
        [Display(Name = "File Extension")]
        public string ExtensionType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Last Changed")]
        public DateTime UpdatedDate { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }


        private string OriSliderPhotoId { get; set; }
        private string OriFileLocation { get; set; }
        private string OriImageDesc { get; set; }
        private string OriFilename { get; set; }
        private string OriExtensionType { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstHomeSlider() { }

        // Retrieve from Db based on PK
        public static MstHomeSlider GetMstHomeSlider(string imgId)
        {
            MstHomeSlider img = new MstHomeSlider();
            img.SliderPhotoId = imgId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, img, filterType: "All");
            List<MstHomeSlider> imgList = (List<MstHomeSlider>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (imgList.Count == 1)
            {
                return imgList[0];
            }
            return null;
        }

        #region Getter/Setter

        public string GetOriFileName()
        {
            return OriFilename;
        }

        public string GetOriFileLocation()
        {
            return OriFileLocation;
        }

        #endregion

        #region Methods

        //? Insert new record
        public int CreateMstHomeSlider(string userName = "")
        {
            this.SliderPhotoId = Guid.NewGuid().ToString();
            this.Status = "Active";
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
            this.CreatedBy = userName;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Insert");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Update existing record
        public int UpdateMstHomeSlider(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstHomeSlider()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Select from database based on value
        public List<MstHomeSlider> SelectMstHomeSlider(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstHomeSlider> data = (List<MstHomeSlider>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstHomeSlider>();
            }
            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstHomeSlider()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        //? Select all records from database
        public static List<MstHomeSlider> SelectMstHomeSlider_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstHomeSlider> data = (List<MstHomeSlider>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstHomeSlider>();
            }
            return data;
        }

        
        #endregion
        //end class
    }
}