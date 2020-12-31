using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Series_watcher.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Series_watcher.Implementation
{
    public class ReportSender : IReportSender
    {
        private readonly IConfiguration _configuration;
        public ReportSender (IConfiguration configuration)
        {
            _configuration = configuration;
            ACCOUNT_SID = _configuration.GetValue<string> ("Twilio:AccountSid");
            AUTH_TOKEN = _configuration.GetValue<string> ("Twilio:AuthToken");
            Email = _configuration.GetValue<string> ("Email");
            Pass = _configuration.GetValue<string> ("Password");
            PROVIDER = _configuration.GetValue<string> ("SmtpDetails:Provider");
            PORT = _configuration.GetValue<int> ("SmtpDetails:Port");
        }

        public string ACCOUNT_SID { get; private set; }
        public string AUTH_TOKEN { get; private set; }
        public string Email { get; private set; }
        public string Pass { get; private set; }
        public string PROVIDER { get; private set; }
        public int PORT { get; private set; }

        public void SendEmail (string message, string recipientEmail)
        {
            var mimeMessage = new MimeMessage ();
            mimeMessage.From.Add (new MailboxAddress ("Tv series watcher", Email));
            mimeMessage.To.Add (new MailboxAddress ("Tv series Report", recipientEmail));

            mimeMessage.Subject = $"Tv shows watcher report dated {DateTime.Now.ToLongDateString()}";
            var body = new TextPart ("plain")
            {
                Text = message
            };

            mimeMessage.Body = body;

            using (var client = new SmtpClient ())
            {
                client.Connect (PROVIDER, PORT, false);
                client.Authenticate (Email, Pass);
                client.Send (mimeMessage);
                client.Disconnect (true);
            }

            //throw new NotImplementedException ();
        }

        public Dictionary<string, string> SendReport (string report, string recipientNumber)
        {
            Dictionary<string, string> MessageStatus = new Dictionary<string, string> ();
            TwilioClient.Init (ACCOUNT_SID, AUTH_TOKEN);

            // var reportChunks = SplitInParts(report, 1550);
            int k = 0;
            double partSize = 1550;
            var reportChunks = report
                .ToLookup (c => Math.Floor (k++ / partSize))
                .Select (e => new String (e.ToArray ()));

            foreach (var item in reportChunks)
            {
                int index = 1;
                var message = MessageResource.Create (
                    body: item.ToString (),
                    from: new Twilio.Types.PhoneNumber ("whatsapp:+14155238886"),
                    to : new Twilio.Types.PhoneNumber ($"whatsapp:{recipientNumber}")
                );

                MessageStatus.Add (item.ToString ().Substring (0, 2), message.Status.ToString ());
                index++;
            }

            return MessageStatus;
        }

    }

}