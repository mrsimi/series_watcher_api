using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Series_watcher.Interfaces;

namespace Series_watcher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesWatcherController : ControllerBase
    {
        private readonly ISeriesRunner _seriesRunner;
        private readonly IReportComposer _reportComposer;
        private readonly IReportSender _reportSender;
        public SeriesWatcherController(ISeriesRunner seriesRunner, IReportComposer reportComposer, IReportSender reportSender)
        {
            _seriesRunner = seriesRunner;
            _reportComposer = reportComposer;
            _reportSender = reportSender;
        }

        [HttpGet("{recieverNumber}")]
        public async Task<IActionResult> GetSeriesReport([FromRoute]string recieverNumber )
        {
            if (string.IsNullOrEmpty(recieverNumber) || !recieverNumber.Contains("+"))
            {
                return BadRequest(ModelState);
            }
            var seriesResult = await _seriesRunner.GetAvailiabilityStatus();

            var recommendationResult = await _seriesRunner.GetRecommendations();

            var report = _reportComposer.ComposeReport(recommendationResult, seriesResult);

            var messageStatus = _reportSender.SendReport(report, recieverNumber);

            return Ok(messageStatus);
        }
    }
}