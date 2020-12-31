using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
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
            SenderEmail = _configuration.GetValue<string> ("EmailDetails:Email");
            AWSSESConfigurationSet = _configuration.GetValue<string> ("AwsSESConfigurationSet");
            // Pass = _configuration.GetValue<string> ("EmailDetails:Password");
            SECRET_KEY = _configuration.GetValue<string> ("AWSCredentials:SecretKey");
            ACCESS_KEY = _configuration.GetValue<string> ("AWSCredentials:AccessKey");
        }

        public string ACCOUNT_SID { get; private set; }
        public string AUTH_TOKEN { get; private set; }
        public string SenderEmail { get; private set; }
        public string AWSSESConfigurationSet { get; private set; }
        public string SECRET_KEY { get; private set; }
        public string ACCESS_KEY { get; private set; }

        public async Task< string> SendEmail (string message, string recipientEmail)
        {
            string EmailSubject = $"Tv shows watcher report dated {DateTime.Now.ToLongDateString()}";
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(ACCESS_KEY, SECRET_KEY);
            using (var client = new AmazonSimpleEmailServiceClient (awsCredentials, RegionEndpoint.USWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = SenderEmail,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { recipientEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content (EmailSubject),
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = message
                            }
                        }
                    },
                    ConfigurationSetName = "WatcherAPI_Config"
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    return "The Report was sent to the email was sent successfully";
                }
                catch(Exception ex)
                {
                    return "There was an error "+ex.Message;
                }
            }
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
                    to : new Twilio.Types.PhoneNumber ($"whatsapp:+{recipientNumber}")
                );

                MessageStatus.Add (item.ToString ().Substring (0, 2), message.Status.ToString ());
                index++;
            }

            return MessageStatus;
        }

    }

}