using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Lab4ASP.Controllers
{
    public class SendEmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SendEmail()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string subject, string body, string fromEmail = "customer@hooja.se")
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.ReplyToList.Add(new MailAddress(fromEmail));
                    mail.To.Add("ullzten@gmail.com");
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("ullzten@gmail.com", "gytlkokttpgomvtn");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                ViewBag.Message = "Email sent successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while sending the email: " + ex.Message;
            }

            return View();
        }

    }
}
