﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.HelperClass
{
    /// <summary>
    /// Class for containing general purpose function
    /// </summary>
    public class GeneralBLL
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string FileDirectory = System.Configuration.ConfigurationManager.AppSettings["UploadDirectoryForPhoto"];

        //? Checks if the user type saved in session matches the expected user type for a given page
        public static bool VerifyAccessRight(object sessionUserType, string validUserType)
        {
            if (sessionUserType == null) { return false; }
            if (sessionUserType.ToString() == "" || sessionUserType.ToString() != validUserType) { return false; }
            return true;
        }

        public enum Status
        {
            Active,
            Inactive
        }

        public static List<SelectListItem> GetStatusDropdownItems()
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "Active",
                    Text= "Active"
                },
            new SelectListItem()
            {
                Value = "Inactive",
                Text = "Inactive"
            }
        };

            return statusList;
        }

        //? Craetes folders in !exists
        public static void MapFilePath(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
    }
}