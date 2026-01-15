using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailExampleRabbit.Services.EmailService
{
   public interface IExtendEmailSender : IEmailSender
    {

        Task SendBulkEmailAsync(string email, string ccmail,string subject, string htmlMessage );

    }
}
