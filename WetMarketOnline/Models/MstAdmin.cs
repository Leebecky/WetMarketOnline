using System;

namespace EWM.Models
{
    public class MstAdmin
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstAdmin));

        string AdminId { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstAdmin() { }

    


    }
}