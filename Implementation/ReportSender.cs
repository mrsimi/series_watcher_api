using System;
using System.Collections.Generic;
using System.Linq;
using Series_watcher.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Series_watcher.Implementation
{
    public class ReportSender : IReportSender
    {
        private readonly string ACCOUNT_SID = "AC2e1e4543f27c1dfb8ada2579aa5f60bf";
        private readonly string AUTH_TOKEN = "826a702f96193d37a0e9604bcfe3fa03";

        public Dictionary<string, string> SendReport(string report, string recipientNumber)
        {
            Dictionary<string, string> MessageStatus = new Dictionary<string, string>();
            TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);

            // var reportChunks = SplitInParts(report, 1550);
            int k = 0;
            double partSize = 1550;
            var reportChunks = report
                .ToLookup(c => Math.Floor(k++ / partSize))
                .Select(e => new String(e.ToArray()));

            foreach (var item in reportChunks)
            {
                int index = 1;
                var message = MessageResource.Create(
                    body: item.ToString(),
                    from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{recipientNumber}")
                );

                MessageStatus.Add(item.ToString().Substring(0,2), message.Status.ToString());
                index++;
            }



            return MessageStatus;
        }

       
    }

  
}