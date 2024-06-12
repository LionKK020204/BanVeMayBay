using BanVeMayBay.Common;
using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace BanVeMayBay.Controllers
{
    public class MyTicketController : Controller
    {
        BANVEMAYBAYPBLEntities db = new BANVEMAYBAYPBLEntities();
        // GET: MyTicket
      
        
        public ActionResult Index()
        {
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (sessionUser == null) { 
                return Redirect("~/Customer/Login"); 
            } else if(sessionUser != null)
            {
            var listOrder = db.orders.Where(m => m.CusId == sessionUser.ID).OrderByDescending(m => m.ID).ToList();
                 return View("index", listOrder);
            }
            return Redirect("~/Customer/Login");
        }
            public ActionResult orderDetailCus(int id)
            {
                var sigleOrder = db.orders.Find(id);
                return View("orderDetailCus", sigleOrder);
            }
            public ActionResult canelOrder(int OrderId)
            {


                order morder = db.orders.Find(OrderId);
                var orderDetail = db.ordersdetails.Where(m => m.orderid == morder.ID).ToList();
                foreach (var item in orderDetail)
                {
                    var id = int.Parse(item.ticketId.ToString());
                    ticket ticket = db.tickets.Find(id);
                    DateTime ngaymuon = Convert.ToDateTime(
                        morder.created_ate);
                    DateTime ngaytra = Convert.ToDateTime(ticket.departure_date);
                    TimeSpan Time = ngaytra - ngaymuon;
                    int TongSoNgay = Time.Days;
                    if (TongSoNgay >= 14)
                    {
                        ticket.Sold = ticket.Sold - item.quantity;
                        db.Entry(ticket).State = EntityState.Modified;
                        db.SaveChanges();
                        if (item == null)
                        {
                            Message.set_flash("Error Cancel Order", "danger");
                            return Redirect("~/tai-khoan");
                        }
                        db.ordersdetails.Remove(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        Message.set_flash("Không thể hủy vé trước ngày bay 14 ngày", "nguy hiểm");
                        return Redirect("~/tai-khoan");
                    }


                }

                db.orders.Remove(morder);
                db.SaveChanges();
                Message.set_flash("Đã hủy 1 đơn hàng", "thành công");
                return Redirect("~/tai-khoan");
            }
        
    }
    }
