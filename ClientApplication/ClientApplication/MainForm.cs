using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using CommManager;

namespace ClientApplication
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            List<MailMessage> messages = new List<MailMessage>()
            {
                new MailMessage(ConfigurationManager.AppSettings["EmailSender"], "arikg94il@gmail.com", "Test Subject", "My message body"),
                new MailMessage(ConfigurationManager.AppSettings["EmailSender"], "arie.gurin@rehnonline.com", "Test Subject", "My message body"),
                new MailMessage(ConfigurationManager.AppSettings["EmailSender"], "arie.gurin@rehnonline.com", "Test Subject", "My message body")
            };

            await EmailsManager.SendEmails(messages);

        }
            
    }
}
