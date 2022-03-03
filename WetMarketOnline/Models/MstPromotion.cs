using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstPromotion
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstPromotion).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstPromotion>).AssemblyQualifiedName;

        public string PromotionId { get; set; }
        [Display(Name = "Promotion Code")]
        public string PromotionCode { get; set; }
        [Display(Name = "Description")]
        public string PromotionDesc { get; set; }
        public decimal? Amount { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Last Updated")]
        public DateTime UpdatedDate { get; set; }
        [Display(Name = "Updated By")]
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


        // Retrieve from Db based on PK
        public static MstPromotion GetMstPromotion(string promotionId)
        {
            MstPromotion promotion = new MstPromotion();
            promotion.PromotionId = promotionId;
            //promotion.Amount = null;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, promotion, filterType: "All");
            List<MstPromotion> promotionList = (List<MstPromotion>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (promotionList.Count == 1)
            {
                return promotionList[0];
            }
            return null;
        }

        #region Getter/Setter

     
        #endregion

        #region Methods

        //? Insert new record
        public int CreateMstPromotion(string userName = "")
        {
            this.PromotionId = Guid.NewGuid().ToString();
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
        public int UpdateMstPromotion(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstPromotion()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Select from database based on value
        public List<MstPromotion> SelectMstPromotion(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstPromotion> data = (List<MstPromotion>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstPromotion>();
            }
            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstPromotion()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        //? Select all records from database
        public static List<MstPromotion> SelectMstPromotion_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstPromotion> data = (List<MstPromotion>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstPromotion>();
            }
            return data;
        }

        #endregion
    }
}