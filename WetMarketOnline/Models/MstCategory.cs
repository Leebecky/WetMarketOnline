using EWM.HelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
namespace EWM.Models
{
    public class MstCategory
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ObjectName = typeof(MstCategory).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstCategory>).AssemblyQualifiedName;

        public string CategoryId { get; set; }
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; }
        [Display(Name = "Description")]
        public string CategoryDesc { get; set; }
        [Display(Name = "Parent Category")]
        public string ParentCatId { get; set; }
        [Display(Name = "Category Level")]
        public int? CatLevel { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Last Updated")]
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        private string OriCategoryId { get; set; }
        private string OriCategoryCode { get; set; }
        private string OriCategoryDesc { get; set; }
        private string OriParentCatId { get; set; }
        private int? OriCatLevel { get; set; }
        private string OriStatus { get; set; }
        private DateTime? OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime? OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstCategory() { }

        // Constructor - Retrieve from Db based on PK
        public static MstCategory GetMstCategory(string catId)
        {
            MstCategory cat = new MstCategory();
            cat.CategoryId = catId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, cat, filterType: "All");
            List<MstCategory> catList = (List<MstCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (catList.Count == 1)
            {
                return catList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstCategory(string userName = "")
        {
            this.CategoryId = Guid.NewGuid().ToString();
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
        public int UpdateMstCategory(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstCategory()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Find Data from table
        public List<MstCategory> SelectMstCategory(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstCategory> data = (List<MstCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstCategory()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }

        //? Select all records from database
        public static List<MstCategory> SelectMstCategory_All()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, null, "SelectAll");
            List<MstCategory> data = (List<MstCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (data == null)
            {
                data = new List<MstCategory>();
            }
            return data;
        }


        //? Get the max number of categories
        public static int GetMaxCat()
        {
            string sql = "Select IsNull(max(cat_level), 1) from mst_category";
            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = DatabaseManager.ExecuteQueryCommand_Datatable(cmd);

            int catLevel = int.Parse(dt.Rows[0][0].ToString());

            return catLevel;
        }

        public static List<MstCategory> GetCategoryDescendents(int catLevel = 1, string parentId = "")
        {
            string sql = "Select mst.category_id, mst.category_code, mst.category_desc, ch.category_code as parent_cat_id, mst.cat_level, mst.status, mst.updated_date, mst.updated_by from mst_category mst  left join mst_category ch on mst.parent_cat_id = ch.category_id where mst.cat_level = @catLevel";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@catLevel", catLevel);

            if (!string.IsNullOrEmpty(parentId))
            {
                sql = "Select mst.category_id, mst.category_code, mst.category_desc, ch.category_code as parent_cat_id, mst.cat_level, mst.status, mst.updated_date, mst.updated_by from mst_category mst  left join mst_category ch on mst.parent_cat_id = ch.category_id where mst.parent_cat_id = @parentId";
                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@parentId", parentId);
            }

            List<MstCategory> catList = (List<MstCategory>)DatabaseManager.ExecuteQueryCommand_Object(cmd, MstCategory.ObjectName, MstCategory.ListName);

            if (catList == null)
            {
                catList = new List<MstCategory>();
            }
            return catList;
        }
        #endregion
    }
}