using System.Net.Mail;
using System.Collections.Generic;
using System;
using System.Net;

namespace SharedClasses
{
    public class SMTP
    {
        SmtpClient client = new SmtpClient();
        bool isBodyHtml = false;

        public SMTP(string _user, string _pw, string _host, int _port, bool _isBodyHtml = false, bool _enableSsl = true)
        {
            isBodyHtml = _isBodyHtml;
            client.Port = _port;
            client.Host = _host;
            client.EnableSsl = _enableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_user, _pw);
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        }

        public bool sendEmail(string _fromAddress, string _toAddresses, string _subject, string _emailBody, string _ccAddresses = "", string _bccAddresses = "", List<Attachment> attachments = null)
        {
            if (_toAddresses == null || _toAddresses == string.Empty)
                return true;

            client.Timeout = 100000;//more than 1 s

            MailMessage message = new MailMessage();

            message.BodyEncoding = System.Text.Encoding.UTF8;

            message.From = new MailAddress(_fromAddress);
            message.To.Add(_toAddresses);
            message.Subject = _subject;
            message.Body = _emailBody;

            if (!string.IsNullOrEmpty(_ccAddresses))
            {
                message.CC.Add(_ccAddresses);
            }
            if (!string.IsNullOrEmpty(_bccAddresses))
            {
                message.Bcc.Add(_bccAddresses);
            }

            message.IsBodyHtml = isBodyHtml;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            if (attachments != null)
                foreach (Attachment data in attachments)
                    message.Attachments.Add(data);

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                throw new Exception(errorMsg) ;
            }
            // Clean up
            message.Dispose();

            return true;
        }

    }
}