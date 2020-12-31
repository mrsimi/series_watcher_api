using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Series_watcher.DTO;
using Series_watcher.Interfaces;

namespace Series_watcher.Implementation
{
    public class ReportComposer : IReportComposer
    {
        public string ComposeReport (List<SeriesRecommendation> recommendations, List<TvSeriesDTO> series)
        {
            var seriesReport = BuildSeriesListReport (series);
            var recommedationReport = BuildRecommendation (recommendations);

            string report = !string.IsNullOrEmpty (seriesReport) ? seriesReport : "None of the series you are watching  new series out this is coming out this week or next week";

            return $"Here is the report on Tv Series you are watching dated {DateTime.Now.ToLongDateString()} \n \n {report}, \n \n and here is the report on series recommendations \n \n {recommedationReport}";
        }

        public string BuildSeriesListReport (List<TvSeriesDTO> series)
        {
            string result = "";
            foreach (var item in series)
            {
                var oneReport = BuildOneSeriesReport (item);
                if (!string.IsNullOrEmpty (oneReport))
                {
                    string comprehensiveReport = $"{item.SeriesTitle} \n" + oneReport;
                    result += comprehensiveReport;
                }
            }

            return result;
        }

        private string BuildOneSeriesReport (TvSeriesDTO series)
        {
            if (series.HasCurrentSeasonEnded)
            {
                string report = $"{series.SeriesTitle} season {series.CurrentSeason} has ended \n\n";
                return report;
            }
            int[] thisWeeks = { 0, -1, -2, -3, -4, -5, -6, -7 };
            int[] nextWeeks = { 1, 2, 3, 4, 5, 6, 7 };

            //if(series.LastEpisodeAirDate == )

            var lastEpisodeDate = series.LastEpisodeAirDate.HasValue ?  series.LastEpisodeAirDate.Value.Subtract (DateTime.Now).Days : 100;
            var newEpisodeDate = series.NewEpisodeAirDate.HasValue? series.NewEpisodeAirDate.Value.Subtract (DateTime.Now).Days : 100;

            string lastEpisodeReportThisWk = thisWeeks.Contains (lastEpisodeDate) != false ?
                $"Season {series.CurrentSeason} episode {series.LastEpisode} is the last available episode and it was aired this week \n" : "";

            string newEpisodeReportThisWk = thisWeeks.Contains (newEpisodeDate) != false ?
                $"Season {series.CurrentSeason} episode {series.NewEpisode} is the new episode which is going to be aired this week \n" : "";

            string lastEpisodeReportNxtWk = nextWeeks.Contains (lastEpisodeDate) != false ?
                $"Season {series.CurrentSeason} episode {series.LastEpisode} is the last episode  which would be aired next week \n" : "";

            string newEpisodeReportNxtWk = nextWeeks.Contains (newEpisodeDate) != false ?
                $"Season {series.CurrentSeason} episode {series.NewEpisode} is the new episode which would be aired next week \n" : "";

            return lastEpisodeReportThisWk + newEpisodeReportThisWk + lastEpisodeReportNxtWk + lastEpisodeReportThisWk;
        }

        public string BuildRecommendation (List<SeriesRecommendation> recommendations)
        {
            string result = "";

            foreach (var item in recommendations)
            {
                string oneRecommendation = BuildOneRecommendationReport (item);
                result += oneRecommendation;
            }

            return result;
        }

        private string BuildOneRecommendationReport (SeriesRecommendation item)
        {
            return $"Check out, {item.Name} \n which was first aired on {item.Date} ,\n the overview \n {item.Overview}. \n the poster: \n{item.Picture} \n \n";
        }
    }
}