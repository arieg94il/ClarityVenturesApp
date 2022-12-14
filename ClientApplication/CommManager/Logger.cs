using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommManager
{
    
    public class Logger
    {
        public static string filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\LoggedEmails.csv";

        
        public string Sender { get; set; }
       
        public string Recipient { get; set; }
        
        public string Subject { get; set; }
        
        public string Body { get; set; }
        
        public Status Status { get; set; }
        
        public int Attempts { get; set; }
        
        public DateTime LastAttempt { get; set; }
        
        public string ErrorMessage { get; set; }

        public Logger() { }
        private Logger(string sender, string recipient, string subject,
            string body, Status status, int attempts, string errorMessage = "")
        {
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Body = body;
            Status = status;
            Attempts = attempts;

            if(!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("Sender,Recipient,Subject,Body,Status,Attempts,LastAttempt,ErrorMessage");
                }
            }

        }

        public static Logger CreateLogger(string sender, string recipient, string subject,
            string body, Status status, int attempts, string errorMessage)
            => new Logger(sender, recipient, subject, body, status, attempts, errorMessage);

        public static void LogEvent(Logger logger)
        {
            logger.Attempts ++;
            logger.LastAttempt = DateTime.Now;

            

            using (StreamWriter sw = File.AppendText(filePath))
            {
                string quote = "\"";
                sw.WriteLine($@"{quote}{logger.Sender}{quote},{quote}{logger.Recipient}{quote},{quote}{logger.Subject}{quote},{quote}{logger.Body}{quote},{quote}{logger.Status}{quote},{quote}{logger.Attempts}{quote},{quote}{logger.LastAttempt}{quote},{quote}{logger.ErrorMessage}{quote}");
            }

        }


        public static List<Logger> GetLogs()
        {
            List<Logger> logs = new List<Logger>();

            string[] lines = File.ReadAllLines(filePath);
            lines = lines.Skip(1).ToArray();

            foreach (var line in lines)
            {
                string[] splitted = line.Split(',');

                string sender = splitted[0].Trim(new char[] { '\"', '"' });
                string recipient = splitted[1].Trim(new char[] { '\"', '"' });
                string subject = splitted[2].Trim(new char[] { '\"', '"' });
                string body = splitted[3].Trim(new char[] { '\"', '"' });
                Status status = (Status)Enum.Parse(typeof(Status), splitted[4].Trim(new char[]{ '\"','"' }));
                int attempts = int.Parse(splitted[5].Trim(new char[] { '\"', '"' }));
                string errorMessage = splitted[6].Trim(new char[] { '\"', '"' });

                Logger log = CreateLogger(sender, recipient, subject, body, status, attempts, errorMessage);

                logs.Add(log);
            }

            return logs;

        }

        

    }
}
