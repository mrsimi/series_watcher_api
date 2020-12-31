using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Series_watcher.Interfaces;

namespace Series_watcher.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class SeriesWatcherController : ControllerBase
    {
        private readonly ISeriesRunner _seriesRunner;
        private readonly IReportComposer _reportComposer;
        private readonly IReportSender _reportSender;
        public SeriesWatcherController (ISeriesRunner seriesRunner, IReportComposer reportComposer, IReportSender reportSender)
        {
            _seriesRunner = seriesRunner;
            _reportComposer = reportComposer;
            _reportSender = reportSender;
        }

        [HttpGet ()]
        public async Task<IActionResult> GetSeriesReport ([FromQuery] string recieverNumber, [FromQuery] string recieverEmail)
        {
            if (string.IsNullOrEmpty(recieverEmail) && string.IsNullOrEmpty(recieverNumber))
            {
                return BadRequest ("Check your input in the route");
            }
            var seriesResult = await _seriesRunner.GetAvailiabilityStatus ();

            var recommendationResult = await _seriesRunner.GetRecommendations ();

            var report = _reportComposer.ComposeReport (recommendationResult, seriesResult);

            if (!string.IsNullOrEmpty (recieverEmail))
            {
                _reportSender.SendEmail (report, recieverEmail);
                return Ok ();
            }
            var messageStatus = _reportSender.SendReport (report, recieverNumber);

            return Ok (messageStatus);
        }
    }
}