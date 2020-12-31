using System.Collections.Generic;
using System.Threading.Tasks;
using Series_watcher.ApiResponses;

namespace Series_watcher.Interfaces
{
    public interface ISeriesQuerier
    {
        Task<int> GetSeriesExternalId(string seriesTitle);
        Task<List<Episode>> GetLastTwoEpisodes(int externalId, int season);
        Task<List<RecommendedResult>> GetTvShowsRecommendations();
    }
}