using System;

namespace Series_watcher.Models
{
    public class TvSeriesListing
    {
        public int Id { get; set; }
        public string SeriesTitle { get; set; }
        public int CurrentSeason { get; set; }
        public int ExternalSeriesId { get; set; }
        public int LastEpisode { get; set; }
        public int NewEpisode { get; set; }
        public DateTime? NewEpisodeAirDate { get; set; }
        public DateTime? LastEpisodeAirDate { get; set; }
    }
}