using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using BanVeMayBay.Models;
using System.Net;
using System.Data.Entity;

namespace BanVeMayBay.Controllers
{
    public class ForgotPassController : Controller
    {
        private BANVEMAYBAYPBLEntities db = new BANVEMAYBAYPBLEntities();

        Random random = new Random();
        int otp;
        // GET: ForgotPass

        public ActionResult Index()
        {

            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            string EnterEmail = fc["enteremail"];
            var user_account = db.users.Where(m => m.access == 1 && m.status == 1 && (m.email == EnterEmail)).FirstOrDefault();
            if (user_account == null)
            {
                ViewBag.succes = "Không tìm thấy email!";
            }
            else
            {
                var email_account = user_account.email;
                otp = random.Next(100000, 1000000);
                Session["OTP"] = otp;
                var fromAddress = new MailAddress("flightpbl@gmail.com");
                var toAddress = new MailAddress(email_account);
                var frompass = "qffggxunsmwlrfhr";
                const string subject = "OTP code";
                string body = otp.ToString();
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, frompass),
                    Timeout = 200000,
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = "Mật khẩu mới là: "+ body
                })
                {
                    smtp.Send(message);
                };
                string psN = otp.ToString();
                user_account.password = psN.ToMD5();
                db.Entry(user_account).State = EntityState.Modified;
                db.SaveChanges();
                
                Message.set_flash("Mật khẩu mới đã được gửi về gmail","success");
                return Redirect("~/dang-nhap");
            }
            return View();
        }
    }
}