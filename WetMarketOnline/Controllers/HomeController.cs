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
    public class HomeController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            MstHomeSlider slider = new MstHomeSlider();
            slider.Status = "Active";
            string directory = MstHomeSlider.FileDirectory;
            List<MstHomeSlider> sliderImages = slider.SelectMstHomeSlider("All");
            ViewData["HomeImgSlider"] = sliderImages;
            return View();
        }

        public ActionResult About()
        {
          
            ViewBag.Message = "About Page";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}