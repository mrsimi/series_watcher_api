using System.Collections.Generic;
using System.Threading.Tasks;
using Series_watcher.DTO;

namespace Series_watcher.Interfaces
{
    public interface IReportComposer
    {
        string ComposeReport(List<SeriesRecommendation> recommendations, List<TvSeriesDTO> series);
    }
}