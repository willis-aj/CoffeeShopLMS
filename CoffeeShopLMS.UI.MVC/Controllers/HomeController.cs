using System;
using System.Web.Mvc;
using CoffeeShopLMS.UI.MVC.Models;
using System.Net.Mail;//added for .NET email (mail message and Smtp Client)
using System.Net;//for network credential


namespace CoffeeShopLMS.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            
            return View();
        }


        [HttpPost]
        public ActionResult Contact(ContactViewModel contactform)
        {
            if (!ModelState.IsValid)
            {
                //bad data didnt follow our meta data req's send back to view with error msgs to try again
                return View(contactform);
            }//end if

            string message = $"This message came from The Coffee Shop Contact Form.<br/><br/>Email from {contactform.Name} at {contactform.Email}. Please respond to that email. <br/><br/>Subject: {contactform.Subject} <br/><br/>Message: <br/>{contactform.Message}<br/><br/>Sent at: {DateTime.Now}";

            //set up mail message obj
            MailMessage mm = new MailMessage(
                "no-reply@abigaylewillis.com",
                "willis.aj@outlook.com",
                $"{contactform.Subject} {DateTime.Now}",
                message);
            mm.IsBodyHtml = true;
            mm.ReplyToList.Add(contactform.Email);

            //make simple mail transfer protocol Smtp client
            SmtpClient emailer = new SmtpClient("mail.abigaylewillis.com");
            emailer.Port = 8889;
            emailer.Credentials = new NetworkCredential("no-reply@abigaylewillis.com", "@1001w");

            try
            {
                emailer.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = $"Sorry something went wrong. Please try again later or contact the admin. <br/>Error message:<br/>{ex.StackTrace}";
                return View(contactform);
            }
            return View("EmailConfirmation", contactform);
        }
    }
}
