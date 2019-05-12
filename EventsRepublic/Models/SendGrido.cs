using Microsoft.IdentityModel.Protocols;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public static class SendGrido
    {
        public static async Task Execute(string img)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var message = new SendGridMessage();
                string cidban = "banner";
                message.AddTo("derobuts@outlook.com");
                message.From = new EmailAddress("tbutoyez@gmail.com", "dero");
                message.Subject = "May The Code Be With You";
                message.PlainTextContent = "Pullup";
                message.HtmlContent = "<html><body><p>This Is SPARTA<p/><img src=cid:" + cidban + "/></body></html>";
                message.AddAttachment("DER", img, "image/png", "inline", "banner");
               
                var response =  client.SendEmailAsync(message);
            }
            catch (WebException e)
            {
                
               var h =  e.Message.ToString();
              
                
            }
            

        }

        public static async Task SendEmailAsync()
        {
            
            try
            {
                await Task.Run(() =>
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.sendgrid.net");
                    mail.From = new MailAddress("xxxxxxxx@gmail.com");
                    mail.To.Add("derobuts@outlook.com");
                    mail.Subject = "Test Mail - 1";
                    mail.Body = "mail with attachment";
                    System.Net.Mail.Attachment attachment;
                    Image img;
                 
                    SmtpServer.Port = 25;
                   
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    internal class ConfigurationManager
    {
    }
}
    



