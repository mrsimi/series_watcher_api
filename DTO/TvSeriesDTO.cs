using Series_watcher.Models;

namespace Series_watcher.DTO
{
    public class TvSeriesDTO : TvSeriesListing
    {
        public bool HasCurrentSeasonEnded { get; set; }
    }
}