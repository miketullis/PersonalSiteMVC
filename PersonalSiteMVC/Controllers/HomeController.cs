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

        #region Ajax Contact Form

        //POST Request
        [HttpPost]
        public JsonResult ContactAjax(ContactViewModel cvm)
        {
            //Lead-in Message 
            string body = $"You have received an email from {cvm.Name}, with a subject of: {cvm.Subject}. Please respond " +
                $"to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

            //MailMessage
            MailMessage mm = new MailMessage(
                //FROM
                ConfigurationManager.AppSettings["EmailUser"].ToString(),
                //TO
                ConfigurationManager.AppSettings["EmailTo"].ToString(),
                //SUBJECT
                cvm.Subject,
                //BODY of the email
                body);

            //MailMessage properties
            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(cvm.Email);

            //SmtpClient
            SmtpClient client = new SmtpClient(
                ConfigurationManager.AppSettings["EmailClient"].ToString());

            //Client credentials
            client.Credentials = new NetworkCredential(
                 ConfigurationManager.AppSettings["EmailUser"].ToString(),
                 ConfigurationManager.AppSettings["EmailPass"].ToString());

            //Client Properties
            client.Port = 8889;

            //Try to send the email
            try
            {
                //Attempt to send the email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                //Format an error message for the user
                ViewBag.Message = $"We're sorry, but your request could not be completed at this time. " +
                    $"Please try again later.<br /> Error Message: <br />{ex.StackTrace}";
            }
            //Send them back to the View with their completed form data
            return Json(cvm);
        }
        #endregion
    }//end class
}//end namespace


