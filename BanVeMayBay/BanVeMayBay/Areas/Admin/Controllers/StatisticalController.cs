using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class StatisticalController : BaseController
    {
        private BANVEMAYBAYPBLEntities db = new BANVEMAYBAYPBLEntities();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            ViewBag.Songuoitruycap = HttpContext.Application["Songuoitruycap"].ToString();
            return View();
            
        }
        
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            
            var query = from o in db.orders where o.status==1
                        join od in db.ordersdetails
                        on o.ID equals od.orderid
                        join t in db.tickets
                        on od.ticketId equals t.id
                        select new
                        {
                            CreateDate = o.created_ate,
                            Quantity = od.quantity,
                            Price = t.price,
                            number=1
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreateDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalQuanty = x.Sum(y => y.Quantity),
                Total = x.Sum(y => y.Quantity * y.Price),
                num=x.Sum(y => y.number)
            }); 
            

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStatisticalyear()
        {
            ViewBag.Songuoitruycap = HttpContext.Application["Songuoitruycap"].ToString();
            return View();
        }
        [HttpGet]
        public ActionResult GetStatisticalyear2(int? year)
        {

            var query = from o in db.orders
                        where o.status == 1 
                        join od in db.ordersdetails
                        on o.ID equals od.orderid
                        join t in db.tickets
                        on od.ticketId equals t.id
                        select new
                        {
                            CreateDate = o.created_ate,
                            Quantity = od.quantity,
                            Price = t.price,
                            number = 1
                        };

            

            if (year!=0)
            {
                query = query.Where(x =>  x.CreateDate.Year == year);
            }

            var result = query.GroupBy(x => new { Year = x.CreateDate.Year, Month = x.CreateDate.Month })
                  .Select(x => new
                  {
                      Year = x.Key.Year,
                      Month = x.Key.Month,
                      TotalQuanty = x.Sum(y => y.Quantity),
                      Total = x.Sum(y => y.Quantity * y.Price),
                      num = x.Sum(y => y.number)
                  });


            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}

    
