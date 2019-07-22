using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace B2BLogical
{
    /// <summary>
    /// Отправка сообщений пользователям.
    /// </summary>
    public class SendMessage
    {
        //http://social.msdn.microsoft.com/Forums/ru-RU/20e66ba6-cd42-4ab3-8be7-7e2457406b9a/send-email-through-gmail-in-c?forum=csharpgeneral
        //Необходимо разрешить отправку почты из других прилождений на GMail. https://www.google.com/settings/security/lesssecureapps

        public static Logger Log = LogManager.GetCurrentClassLogger();

        private static MailAddress _fromAddress = null;
        private static MailAddress FromAddress
        {
            get
            {
                if (_fromAddress == null)
                {
                    Init();
                }
                return _fromAddress;
            }
        }

        private static void Init()
        {
            SetupMail setup = new SetupMail();

            _fromAddress = new MailAddress(setup.FromMail, setup.FromName);

            _smtp = new SmtpClient();
            _smtp.Host = setup.SMTPHost;
            if (setup.SMTPPort.HasValue)
                _smtp.Port = setup.SMTPPort.Value;
            if (setup.SMTPEnableSSL.HasValue)
                _smtp.EnableSsl = setup.SMTPEnableSSL.Value;
            _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtp.UseDefaultCredentials = setup.SMTPUseDefaultCredentials;
            if (!setup.SMTPUseDefaultCredentials)
            {
                _smtp.Credentials = new System.Net.NetworkCredential(FromAddress.Address, setup.FromPassword);
            }
        }

        private static SmtpClient _smtp = null;

        private static SmtpClient SMTP
        {
            get
            {
                if (_smtp == null)
                {
                    Init();
                }
                return _smtp;
            }
        }

        /// <summary>
        /// Отправка писем по адресу.
        /// </summary>
        /// <param name="email">Список адресов, куда отправлять</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело</param>
        /// <param name="fileName">Приложение</param>
        /// <returns></returns>
        public static bool SendGmailCom(List<RecipientEmail> email, string subject, string body, string fileName)
        {
            //MailAddress toAddress = new MailAddress(email, name);
            //using (MailMessage message = new MailMessage(FromAddress, toAddress))//Иначе нельзя удалить файл после отправки
            using (MailMessage message = new MailMessage())//Иначе нельзя удалить файл после отправки
            {
                message.From = FromAddress;
                StringBuilder listEmailLog = new StringBuilder();
                foreach (var toEmail in email)
                {
                    MailAddress toAddress = new MailAddress(toEmail.Email);
                    message.To.Add(toAddress);
                    listEmailLog.Append(toAddress.Address + ";");
                }
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(fileName))
                    message.Attachments.Add(new Attachment(fileName));
                //message.Body = "<html><b>Hello World!</b></html>";            // SEND EMAIL
                try
                {
                    //if (email[0].CustomerNo == "TEST_GRIGORYEV")
                    SMTP.Send(message);
                }
                catch (Exception exc)
                {
                    Log.Error(exc, "Ошибка при отправке сообщения {0} на адрес {1}.", subject, email[0].Email + ": " + listEmailLog + ". Error: " + exc.Message);
                    return false;
                }
                Log.Trace(String.Format("Сообщение {0} было отправлено {1}", subject, email[0].Email + ": " + listEmailLog));
                return true;
            }
        }
    }
}
