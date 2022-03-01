using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EWM.Controllers
{
    public class MstHomeSliderController : Controller
    {
        //? MstAdmin Manage Home Slider Images Page
        public ActionResult ManageHomeSlider()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Admin")) { return RedirectToAction("Login", "Account"); }

            List<MstHomeSlider> imgList = MstHomeSlider.SelectMstHomeSlider_All();

            return View(imgList);
        }

        // GET: MstHomeSlider/Details/5
        public ActionResult Details(string id, string mode)
        {

            ViewData["PageMode"] = mode;
            if (mode == "New")
            {
                return View("HomeSlider_ImageDetails", new MstHomeSlider());
            }
            else
            {
                MstHomeSlider slider = MstHomeSlider.GetMstHomeSlider(id);
                return View("HomeSlider_ImageDetails", slider);
            }
        }

        [HttpPost]
        //? Update/Add new MstHomeSlider record
        public ActionResult UpdateMstHomeSlider(MstHomeSlider formData, HttpPostedFileBase FormFileUpload, string mode)
        {
            MstAdmin member = (MstAdmin)Session["Account"];
            string fileExtension = "";
            string filePath = "";

            //Get absolute Upload path 
            string UploadPath = Server.MapPath(MstHomeSlider.FileDirectory);

            // File Path Configuration
            if (FormFileUpload != null)
            {
                //string fileName = Path.GetFileNameWithoutExtension(FormFileUpload.FileName);      
                string fileName = formData.Filename;
                fileExtension = Path.GetExtension(FormFileUpload.FileName);

                //Add Current Date To Attached File Name : Filename_20220226.png
                string completeFileName = fileName.Trim() + "_" + DateTime.Now.ToString("yyyyMMdd") + fileExtension;

                //Create relative path to store in server.  
                filePath = typeof(MstHomeSlider).Name + "/" + completeFileName;

                //To copy and save file into server.  
                FormFileUpload.SaveAs(String.Concat(UploadPath, filePath));


                // Set to form data values
                formData.ExtensionType = fileExtension;
                formData.FileLocation = filePath;
            }

            if (mode == "Edit")
            {
                MstHomeSlider oriSlider = MstHomeSlider.GetMstHomeSlider(formData.SliderPhotoId);
                oriSlider.Filename = formData.Filename;
                oriSlider.Status = formData.Status;
                oriSlider.ImageDesc = formData.ImageDesc;

                oriSlider.FileLocation = formData.FileLocation;
                oriSlider.ExtensionType = formData.ExtensionType;

                if (FormFileUpload == null) //if no file was uploaded
                {
                    string newFilePath = formData.FileLocation.Replace(oriSlider.GetOriFileName(), formData.Filename);
                    System.IO.File.Move(string.Concat(UploadPath, formData.FileLocation), string.Concat(UploadPath, newFilePath));

                    oriSlider.FileLocation = newFilePath;
                }
                oriSlider.UpdateMstHomeSlider(member.Username);
            }
            else if (mode == "New")
            {
                formData.CreateMstHomeSlider(member.Username);
            }
            return RedirectToAction("ManageHomeSlider");
        }

        // GET: MstHomeSlider/Delete/5
        public ActionResult Delete(string id)
        {
            MstHomeSlider slider = MstHomeSlider.GetMstHomeSlider(id);
            int rowsAffected = slider.DeleteMstHomeSlider();

            string UploadPath = Server.MapPath(MstHomeSlider.FileDirectory);
            System.IO.File.Delete(String.Concat(UploadPath, slider.FileLocation));

            if (rowsAffected == 1)
            {
                return RedirectToAction("ManageHomeSlider");
            }
            else
            {
                ViewBag.Error = "Failed to delete the image. Please try again.";
                return View();
            }
        }

        //end class
    }
}
