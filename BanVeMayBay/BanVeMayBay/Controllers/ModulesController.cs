using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class ModulesController : Controller
    {
        // GET: Modules
        BANVEMAYBAYPBLEntities db = new BANVEMAYBAYPBLEntities();
        
        public ActionResult _Mainmenu()
        {
            if ((string)Session["userName11"] != "")
            {
                ViewBag.sessionFullname = Session["userName11"];
            }
            else
            {

            }
            
            return View("_Mainmenu");
        }
        public ActionResult _Footer()
        {
            return View("_Footer");
        }

        public ActionResult Slider()
        {
            return View("Slider");
        }
        public ActionResult LogoSlide()
        {
            return View("LogoSlide");
        }
    }
}