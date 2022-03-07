using EWM.HelperClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EWM.Models
{
    public class MstMerchant
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstMerchant).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstMerchant>).AssemblyQualifiedName;

        public string MerchantId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

       
        // Original Data
        private string OriMerchantId { get; set; }
        private string OriUsername { get; set; }
        private string OriPassword { get; set; }
        private string OriName { get; set; }
        private string OriEmail { get; set; }
        private string OriAddress { get; set; }
        private string OriState { get; set; }
        private string OriPostcode { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstMerchant() { }

        // Constructor - Retrieve from Db based on PK
        public static MstMerchant GetMstMerchant(string merchantId)
        {
            MstMerchant merchant = new MstMerchant();
            merchant.MerchantId = merchantId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, merchant, filterType: "All");
            List<MstMerchant> merchantList = (List<MstMerchant>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (merchantList.Count == 1)
            {
                return merchantList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstMerchant(string userName = "")
        {
            this.MerchantId = Guid.NewGuid().ToString();
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
        public int UpdateMstMerchant(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstMerchant()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Insert new record
        public List<MstMerchant> SelectMstMerchant(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstMerchant> data = (List<MstMerchant>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstMerchant()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }
        #endregion


    }
}