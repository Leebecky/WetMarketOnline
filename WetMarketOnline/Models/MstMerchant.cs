﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWM.Models
{
    public class MstMerchant
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MstMerchant));

        string MerchantId { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Address { get; set; }
        string State { get; set; }
        string Postcode { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        // Default Constructor
        public MstMerchant() { }

    


    }
}