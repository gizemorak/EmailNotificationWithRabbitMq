using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;

namespace EmailExampleRabbit.Services.EmailService
{
    public class EmailSender : IExtendEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _username;
        private string _password;
        public EmailSender(string host, int port, bool enableSSL, string username, string password)
        {
            this._host = host;
            this._port = port;
            this._enableSSL = enableSSL;
            this._username = username;
            this._password = password;
        }

        public Task SendBulkEmailAsync(string email,string ccmail, string subject, string htmlMessage)
        {
            try
            {
                var client = new System.Net.Mail.SmtpClient(this._host, this._port)
                {

                    Credentials = new System.Net.NetworkCredential(_username, _password),
                    EnableSsl = this._enableSSL,
                    


                };

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                MailMessage mailMessage = new MailMessage();
                mailMessage.Body = htmlMessage;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = subject;
                mailMessage.From = new MailAddress(this._username);



                mailMessage.To.Add(email);
                mailMessage.CC.Add(ccmail);




                return client.SendMailAsync(mailMessage); ;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            try
            {
                var client = new System.Net.Mail.SmtpClient(this._host, this._port)
                {
                   
                    Credentials = new System.Net.NetworkCredential(_username, _password),
                    EnableSsl = this._enableSSL
                   

            };



                return client.SendMailAsync(
                   (new System.Net.Mail.MailMessage(this._username, email, subject, htmlMessage)
                   {
                       IsBodyHtml = true

                   })
                );
            }
            catch (Exception)
            {

                throw new Exception("Email gönderilirken bir hata oluştu.");
            }
          
        }

       
    }
}
