using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EWM.HelperClass;
using log4net;

namespace EWM.Models
{
    public class MstCustomer
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstCustomer).AssemblyQualifiedName;
        public static string ListName = typeof(List<MstCustomer>).AssemblyQualifiedName;

        public string CustomerId { get; set; }
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


        private string OriCustomerId { get; set; }
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
        public MstCustomer() {}

        // Constructor - Retrieve from Db based on PK
        public static MstCustomer GetMstCustomer(string customerId)
        {
            MstCustomer customer = new MstCustomer();
            customer.CustomerId = customerId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, customer, filterType: "All");
            List<MstCustomer> customerList = (List<MstCustomer>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            if (customerList.Count == 1)
            {
                return customerList[0];
            }
            return null;
        }

        #region Methods

        //? Insert new record
        public int CreateMstCustomer(string userName = "")
        {
            this.CustomerId = Guid.NewGuid().ToString();
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
        public int UpdateMstCustomer(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstCustomer()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd);

            return rowsAffected;
        }

        //? Insert new record
        public List<MstCustomer> SelectMstCustomer(string filterType = "Column")
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType);
            List<MstCustomer> data = (List<MstCustomer>)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName, ListName);

            return data;
        }

        //? Runs a Select statement and returns the number of rows found
        public int CheckMstCustomer()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Select", filterType: "All");
            int data = DatabaseManager.ExecuteQueryCommand_RowsAffected(cmd, true);

            return data;
        }
        #endregion

    }
}