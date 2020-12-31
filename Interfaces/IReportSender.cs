using System.Collections.Generic;
using System.Threading.Tasks;

namespace Series_watcher.Interfaces
{
    public interface IReportSender
    {
        Dictionary<string, string> SendReport (string message, string recipientNumber);

        Task<string> SendEmail (string message, string recipientEmail);
    }
}