using System.Collections.Generic;
namespace Series_watcher.Interfaces
{
    public interface IReportSender
    {
        Dictionary<string, string> SendReport(string message, string recipientNumber);
    }
}