using System.Collections.Generic;
using System.Threading.Tasks;
using Series_watcher.DTO;

namespace Series_watcher.Interfaces
{
    public interface ISeriesRunner
    {
        Task<List<TvSeriesDTO>> GetAvailiabilityStatus();
        Task<List<SeriesRecommendation>> GetRecommendations();
    }
}