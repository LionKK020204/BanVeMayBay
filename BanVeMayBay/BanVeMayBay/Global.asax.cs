using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanVeMayBay
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Application["Songuoitruycap"] = 0;
        }
        
        protected void Session_Start()
        {
            Application.Lock();//đồng bộ dữ liệu
            Application["Songuoitruycap"] = (int)Application["Songuoitruycap"] + 1;
            Application.UnLock();
            Session["cart"] = "";
            Session["favoriteProduct"] = "";
            Session["Admin_id"] = "";
            Session["Admin_user"] = "";
            Session["Admin_fullname"] = "";
            Session["userName11"] = "";
            Session["Message"] = "";
            Session["id"] = "";
            Session["user"] = "";

        }
    }
}
