using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommManager
{
    public static class EmailsManager
    {

        public static async Task SendEmails(List<MailMessage> emailsMessages)
        {
            foreach(var emailMessage in emailsMessages)
            {
                Logger logger = Logger.CreateLogger(emailMessage.From.Address, emailMessage.To.ToString(), emailMessage.Subject, emailMessage.Body, Status.Not_Sent, 0, "");
                try
                {
                    MailMessage message = new MailMessage(logger.Sender, logger.Recipient, logger.Subject, logger.Body);
                    await Send(message);
                    logger.Status = Status.Sent;
                }
                catch (Exception ex)
                {
                    logger.Status = Status.Failed;
                    logger.ErrorMessage = ex.Message;
                }
                finally
                {
                    Logger.LogEvent(logger);
                    
                }
            }


            if (Logger.GetLogs().Where(x => x.Attempts < 4 && x.Status == Status.Failed).Any())
                while(Logger.GetLogs().Where(x => x.Attempts < 4 && x.Status == Status.Failed).Count()>0)
                    await ResendFailedEmails();


        }

        public static async Task Send(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.SendAsync(message, "token");
        }


        

        private static async Task ResendFailedEmails()
        {

            List<Logger> failedEmails = Logger.GetLogs().Where(x => x.Attempts < 4 && x.Status == Status.Failed).ToList();

            foreach(var logger in failedEmails)
            {
                
                try
                {
                    MailMessage message = new MailMessage(logger.Sender, logger.Recipient, logger.Subject, logger.Body);
                    await Send(message);
                    logger.Status = Status.Sent;
                }
                catch (Exception ex)
                {
                    logger.Status = Status.Failed;
                    logger.ErrorMessage = ex.Message;
                }
                finally
                {
                    Logger.LogEvent(logger);
                }
            }

            

        }
    }
}
