using EWM.HelperClass;
using EWM.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class AccountController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //? Login page
        public ActionResult Login()
        {
            if ( Session["AccountType"] != null){
                string userType = "Mst" + Session["AccountType"].ToString();
                return RedirectToAction(userType, userType);
            }

            return View();
        }

        //? Verifies the user's credentials and sets the Session variables
        [HttpPost]
        public ActionResult SignIn(string username, string password, string userType)
        {
            bool loginValid = false;
            int rowsAffected = -1;

            //? Check Login
            switch (userType.ToLower())
            {
                case "customer":
                    MstCustomer customerAcc = new MstCustomer
                    {
                        Username = username,
                        Password = password,
                        Status = "Active"
                    };

                    rowsAffected = customerAcc.CheckMstCustomer();
                    if (rowsAffected == 1)
                    {
                        Session["Account"] = customerAcc;
                        loginValid = true;
                    }
                    break;

                case "merchant":
                    MstMerchant merchantAcc = new MstMerchant
                    {
                        Username = username,
                        Password = password,
                        Status = "Active"
                    };

                    rowsAffected = merchantAcc.CheckMstMerchant();
                    if (rowsAffected == 1)
                    {
                        Session["Account"] = merchantAcc;
                        loginValid = true;
                    }
                    break;

                case "admin":
                    MstAdmin adminAcc = new MstAdmin
                    {
                        Username = username,
                        Password = password,
                        Status = "Active"
                    };

                    rowsAffected = adminAcc.CheckMstAdmin();
                    if (rowsAffected == 1)
                    {
                        Session["Account"] = adminAcc;
                        loginValid = true;
                    }
                    break;
            }

            if (loginValid)
            {
                Session["AccountType"] = userType;

                switch (userType.ToLower())
                {
                    case "customer":
                        return RedirectToAction("MstCustomer", "MstCustomer");
                    case "merchant":
                        return RedirectToAction("MstMerchant", "MstMerchant");
                    case "admin":
                        return RedirectToAction("MstAdmin", "MstAdmin");
                    default:
                        ViewBag.Error = "Unexpected Error Encountered";
                        return View("Login");
                }
            }
            else
            {
                ViewBag.Error = "Login Failed";
                return View("Login");
            }
        }


        //? Clears the sessions and redirects the user to the Login page
        public ActionResult Logout()
        {
            Session.Remove("Account");
            Session.Remove("AccountType");
            return RedirectToAction("Login");
        }

        //? Sign Up page
        public ActionResult SignUp()
        {
            return View();
        }

        //? Verifies the username and create a new user
        [HttpPost]
        public ActionResult AccountCreation(string username, string password, string userType, string address, string state, string postcode)
        {
            bool validUsername = false;
            int rowsAffected = -1;

            //? Check if username is in use
            switch (userType.ToLower())
            {
                case "customer":
                    MstCustomer customerAcc = new MstCustomer
                    {
                        Username = username,
                        Password = password
                    };

                    rowsAffected = customerAcc.CheckMstCustomer();
                    if (rowsAffected == 0)
                    {
                        validUsername = true;
                    }
                    break;

                case "merchant":
                    MstMerchant merchantAcc = new MstMerchant
                    {
                        Username = username,
                        Password = password
                    };

                    rowsAffected = merchantAcc.CheckMstMerchant();
                    if (rowsAffected == 0)
                    {
                        validUsername = true;
                    }
                    break;
            }

            if (!validUsername)
            {
                ViewBag.Error = "This username is in use. Please choose a different username";
                return View("SignUp");
            } else
            {
                bool accCreationSuccess = false;
                switch (userType.ToLower())
                {
                    case "customer":
                        MstCustomer customerAcc = new MstCustomer
                        {
                            Username = username,
                            Password = password,
                            Address = address,
                            State = state,
                            Postcode = postcode
                        };

                        rowsAffected = customerAcc.CreateMstCustomer(username);
                        if (rowsAffected == 1)
                        {
                            accCreationSuccess = true;
                            Session["Account"] = customerAcc;
                        }
                        break;

                    case "merchant":
                        MstMerchant merchantAcc = new MstMerchant
                        {
                            Username = username,
                            Password = password,
                            Address = address,
                            State = state,
                            Postcode = postcode
                        };

                        rowsAffected = merchantAcc.CreateMstMerchant(username);
                        if (rowsAffected == 1)
                        {
                            accCreationSuccess = true;
                            Session["Account"] = merchantAcc;
                        }
                        break;
                }

                if (accCreationSuccess)
                {
                    Session["AccountType"] = userType;
                    return RedirectToAction("Mst" + userType, "Mst" + userType);
                } else
                {
                    ViewBag.Error = "Unexpected error encountered. Account not created. Please try again";
                    return View("SignUp");
                }
            }
        }


        //end class
    }
}