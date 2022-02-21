using EWM.HelperClass;
using System;
using System.Data.SqlClient;

namespace EWM.Models
{
    public class MstAdmin
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ObjectName = typeof(MstAdmin).AssemblyQualifiedName;

        public string AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        private string OriAdminId { get; set; }
        private string OriUsername { get; set; }
        private string OriPassword { get; set; }
        private string OriName { get; set; }
        private string OriEmail { get; set; }
        private string OriStatus { get; set; }
        private DateTime OriCreatedDate { get; set; }
        private string OriCreatedBy { get; set; }
        private DateTime OriUpdatedDate { get; set; }
        private string OriUpdatedBy { get; set; }

        // Default Constructor
        public MstAdmin() { }

        // Constructor - Retrieve from Db based on PK
        public static MstAdmin GetMstAdmin(string adminId)
        {
            MstAdmin admin = new MstAdmin();
            admin.AdminId = adminId;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, admin, filterType: "All");
            admin = (MstAdmin)DatabaseManager.ExecuteQueryCommand_Object(cmd, ObjectName);

            return admin;
        }

        #region Methods

        //? Insert new record
        public int CreateMstAdmin(string userName = "")
        {
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
            this.CreatedBy = userName;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Insert");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand(cmd);

            return rowsAffected;
        }
        
        //? Update existing record
        public int UpdateMstAdmin(string userName = "")
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;

            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Update");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand(cmd);

            return rowsAffected;
        }

        //? Delete existing record
        public int DeleteMstAdmin()
        {
            SqlCommand cmd = DatabaseManager.ConstructSqlCommand(ObjectName, this, "Delete");
            int rowsAffected = DatabaseManager.ExecuteQueryCommand(cmd);

            return rowsAffected;
        }

        #endregion
    }
}