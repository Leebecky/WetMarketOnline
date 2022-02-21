using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWM.Models
{
    public class MstMerchant
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ObjectName = typeof(MstMerchant).AssemblyQualifiedName;

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

    


    }
}