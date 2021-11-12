using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PersonalSiteMVC.Models;
using System.Net; 
using System.Net.Mail;

namespace PersonalSiteMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult References()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Contact(ContactViewModel cvm)
        {
            //when a class has validation attributes, that validation should be checked before attempting to process any data.
            if (!ModelState.IsValid)
            {
                //something the used typed, or didn't type is causing issues... let's send them the form back with all thier values they typed in to each field
                return Json(cvm);
            }

            //Assemble the message itself - this it what we see in the body of the message being sent from the site
            string message = $"You have received an email from {cvm.Name}: <br/>" +
                $"Subject: {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}<br/>" +
                $"Email: {cvm.Email}<br/>" +
                $"Message:<br/>{cvm.Message}";

            //Then, we can assemble a MailMessage to an staged and used in the SMTPClient when we sent the message

            MailMessage mm = new MailMessage(
                //From
                ConfigurationManager.AppSettings["EmailUser"].ToString(),

                //To - this assumes forwarding by the host. What email do we want this message to send to
                ConfigurationManager.AppSettings["EmailTo"].ToString(),

                //subject
                cvm.Subject,

                //Body
                message
                );

            //MailMessage properties
            mm.IsBodyHtml = true;

            //make the message a high priority
            mm.Priority = MailPriority.High;

            //respond to user's email
            mm.ReplyToList.Add(cvm.Email);

            //SMTP Client object -  creds stored in web.config
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());

            //Add in client credentials
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPass"].ToString());

            client.Port = 8889;

            //Use a try/catch to sent the object in case of mail service or configuration issues.
            try
            {
                //attempt to send the message 
                client.Send(mm);
            }
            catch (Exception ex)
            {
                //if something goes wrong populate a ViewBag message to inform user that the message did not send
                ViewBag.ContactMessage = $"Rour request could not be completed at this time. <br/>" +
                    $"Please try again later.<br/>" +
                    $"Error: {ex.StackTrace}";

                //send the view back with what they have written into the inputs
                return Json(cvm);
            }
            //Return the confirmation view
            return Json("EmailConfirmation" + "<br/>" + cvm);
        }

    }//end class
}//end namespace


