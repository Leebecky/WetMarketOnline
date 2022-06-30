using EWM.HelperClass;
using EWM.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            if (Session["AccountType"] != null)
            {
                string userType = "Mst" + Session["AccountType"].ToString();
                return RedirectToAction(userType, userType);
            }

            ViewBag.Message = TempData["pgMsg"];
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
                        customerAcc = customerAcc.SelectMstCustomer("All")[0];
                        TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(customerAcc.CustomerId);
                        Session["ShoppingCart"] = cart.GetCartItems().Count;
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
                        merchantAcc = merchantAcc.SelectMstMerchant("All")[0];
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
                        adminAcc = adminAcc.SelectMstAdmin("All")[0];
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
            Session.Remove("ShoppingCart");
            return RedirectToAction("Login");
        }

        //? Sign Up page
        public ActionResult SignUp()
        {
            ViewBag.Error = TempData["Error"];
            return View();
        }

        //? Verifies the username and create a new user
        [HttpPost]
        public ActionResult AccountCreation(string username, string password, string email, string userType, string address, string state, string postcode)
        {
            bool validUsername = false;
            bool validEmail = false;
            int rowsAffected = -1;

            //? Check if email is in use
            switch (userType.ToLower())
            {
                case "customer":
                    MstCustomer customerAcc = new MstCustomer
                    {
                        Email = email,
                    };

                    rowsAffected = customerAcc.CheckMstCustomer();
                    if (rowsAffected == 0)
                    {
                        validEmail = true;
                    }
                    break;

                case "merchant":
                    MstMerchant merchantAcc = new MstMerchant
                    {
                        Email = email,
                    };

                    rowsAffected = merchantAcc.CheckMstMerchant();
                    if (rowsAffected == 0)
                    {
                        validEmail = true;
                    }
                    break;
            }

            if (!validEmail)
            {
                TempData["Error"] = "There is already an account belonging to this email!";
                return RedirectToAction("SignUp");
            }

            //? Check if username is in use
            switch (userType.ToLower())
            {
                case "customer":
                    MstCustomer customerAcc = new MstCustomer
                    {
                        Username = username,
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
                TempData["Error"] = "This username is in use. Please choose a different username";
                return RedirectToAction("SignUp");
            }
            else
            {
                bool accCreationSuccess = false;
                switch (userType.ToLower())
                {
                    case "customer":
                        MstCustomer customerAcc = new MstCustomer
                        {
                            Username = username,
                            Name = username,
                            Password = password,
                            Email = email,
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
                            Name = username,
                            Password = password,
                            Email = email,
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
                }
                else
                {
                    TempData["Error"] = "Unexpected error encountered. Account not created. Please try again";
                    return RedirectToAction("SignUp");
                }
            }
        }

        //? Reset password of user + send the new password through email
        [HttpPost]
        public ActionResult ForgotPassword(string resetEmail, string resetUserType)
        {
            //? 
            string msg = "";
            int userFound = -1;

            // Check for User
            switch (resetUserType)
            {
                case "Admin":
                    MstAdmin admin = new MstAdmin();
                    admin.Email = resetEmail;
                    userFound = admin.CheckMstAdmin();
                    break;

                case "Merchant":
                    MstMerchant merchant = new MstMerchant();
                    merchant.Email = resetEmail;
                    userFound = merchant.CheckMstMerchant();
                    break;

                case "Customer":
                    MstCustomer customer = new MstCustomer();
                    customer.Email = resetEmail;
                    userFound = customer.CheckMstCustomer();
                    break;
            }


            if (userFound == 1)
            {
                string newPassword = Guid.NewGuid().ToString().Substring(0, 10);
                int updateSuccess = -1;
                //reset password
                switch (resetUserType)
                {
                    case "Admin":
                        MstAdmin admin = new MstAdmin();
                        admin.Email = resetEmail;
                        admin = admin.SelectMstAdmin("All")[0];
                        admin.Password = newPassword;
                        updateSuccess = admin.UpdateMstAdmin();
                        break;

                    case "Merchant":
                        MstMerchant merchant = new MstMerchant();
                        merchant.Email = resetEmail;
                        merchant = merchant.SelectMstMerchant("All")[0];
                        merchant.Password = newPassword;
                        updateSuccess = merchant.UpdateMstMerchant();
                        break;

                    case "Customer":
                        MstCustomer customer = new MstCustomer();
                        customer.Email = resetEmail;
                        customer = customer.SelectMstCustomer("All")[0];
                        customer.Password = newPassword;
                        updateSuccess = customer.UpdateMstCustomer();
                        break;
                }

                if (updateSuccess == 1)
                {
                    //send mail
                    MailMessage mail = new MailMessage();
                    mail.To.Add(resetEmail);
                    mail.From = new MailAddress("fam.lee2866@gmail.com");
                    mail.Subject = "EWM - Password Reset";
                    string Body = $"Your password has been reset to {newPassword}.";
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("fam.lee2866@gmail.com", "Photo Storage"); // Enter seders User name and password       
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    msg = "Please check your inbox for your new password";
                }
                else
                {
                    msg = "Failed to reset password. Please try again";
                }
            }
            else
            {
                msg = "No user with this email found. Please ensure that you have selected the correct user type";
            }

            TempData["pgMsg"] = msg;
            return RedirectToAction("Login");
        }

        //end class
    }
}