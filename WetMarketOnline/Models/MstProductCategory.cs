using EWM.HelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
namespace EWM.Models
{
    public class MstProductCategory
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ObjectName = typeof(MstProductCategory).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstProductCategory>).AssemblyQualifiedName;

        public string ProductCatId { get; set; }
        public string ProductId { get; set; }
        public string CatId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriProductCatId { get; set; }
        private string OriProductId { get; set; }
        private string OriCatId { get; set; }
        private string OriStatus { get; set; }
        private DateTime? OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime? OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstProductCategory() { }

        // Constructor - Retrieve from Db based on PK
        public static MstProductCategory GetMstProductCategory(string catId)
        {
            MstProductCategory cat = new MstProductCategory();
            cat.ProductCatId = catId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, cat, filterType: "All");
            List<MstProductCategory> catList = (List<MstProductCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (catList.Count == 1)
            {
                return catList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstProductCategory(string userName = "")
        {
            this.ProductCatId = Guid.NewGuid().ToString();
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
        public int UpdateMstProductCategory(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstProductCategory()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Find Data from table
        public List<MstProductCategory> SelectMstProductCategory(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstProductCategory> data = (List<MstProductCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstProductCategory()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        //? Select all records from database
        public static List<MstProductCategory> SelectMstProductCategory_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstProductCategory> data = (List<MstProductCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstProductCategory>();
            }
            return data;
        }


        #endregion
    }
}