using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Series_watcher.ApiResponses;
using Series_watcher.Data;
using Series_watcher.Interfaces;

namespace Series_watcher.Implementation
{
    public class SeriesQuerier : ISeriesQuerier
    {

        private readonly IConfiguration _config;
        public SeriesQuerier (IConfiguration config)
        {
            _config = config;
            TMDB_KEY = _config.GetValue<string> ("TMDBKEY");
        }
        private readonly static string BASE_URL = "https://api.themoviedb.org/3/";
        private static HttpClient client = new HttpClient();

        public string TMDB_KEY { get; private set; }

        public async Task<List<Episode>> GetLastTwoEpisodes (int externalId, int season)
        {
            if (externalId != 0 && season != 0)
            {
                List<Episode> Episodes = new List<Episode> ();
                string referenceUrl = $"tv/{externalId}/season/{season}?language=en-US&api_key={TMDB_KEY}";
                using (var client = new HttpClient ())
                {
                    client.BaseAddress = new System.Uri (BASE_URL);
                    HttpResponseMessage response = await client.GetAsync (referenceUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var deserializedResponse = JsonConvert.DeserializeObject<SeriesDetailResponse> (await response.Content.ReadAsStringAsync ());
                        var lastAvailableEpisode = deserializedResponse.episodes
                            .FindLast (m => !string.IsNullOrEmpty (m.overview) && !string.IsNullOrEmpty (m.air_date) && DateTime.Parse (m.air_date).Subtract (DateTime.Now).Days < 14);
                        lastAvailableEpisode.crew = null;
                        lastAvailableEpisode.guest_stars = null;
                        Episodes.Add (lastAvailableEpisode);
                        int indexofLastAvailableEpisode = deserializedResponse.episodes.IndexOf (lastAvailableEpisode);

                        if (deserializedResponse.episodes.Count != indexofLastAvailableEpisode + 1)
                        {
                            var newEpisode = deserializedResponse.episodes[indexofLastAvailableEpisode + 1];
                            newEpisode.crew = null;
                            newEpisode.guest_stars = null;
                            Episodes.Add (newEpisode);
                        }
                    }
                }

                return Episodes;
            }
            return null;
        }

        public async Task<int> GetSeriesExternalId (string seriesTitle)
        {
            if (!string.IsNullOrEmpty (seriesTitle))
            {
                string referenceUrl = $"search/tv?api_key={TMDB_KEY}&language=en-US&page=1&include_adult=false&query={seriesTitle.Replace(' ', '+')}";
                // using (var client = new HttpClient ())
                // {
                    client.BaseAddress = new System.Uri (BASE_URL);
                    HttpResponseMessage response = await client.GetAsync (referenceUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var deserializedResponse = JsonConvert.DeserializeObject<SearchTvSeriesResponse> (await response.Content.ReadAsStringAsync ());

                        foreach (var result in deserializedResponse.results)
                        {
                            if (result.name.Contains (seriesTitle, System.StringComparison.OrdinalIgnoreCase))
                            {
                                return result.id;
                            }
                        }
                    }
                //}
            }
            return 0;
        }

        public async Task<List<RecommendedResult>> GetTvShowsRecommendations ()
        {
            string[] sort_bys = { "vote_average.desc", "vote_average.asc", "first_air_date.desc", "irst_air_date.asc", "popularity.desc", "popularity.asc" };
            var random = new Random ();
            int index = random.Next (sort_bys.Length);

            string referenceUrl = $"discover/tv?api_key={TMDB_KEY}&language=en-US&sort_by={sort_bys[index]}&page=1&include_null_first_air_dates=false";
            // using (var client = new HttpClient ())
            // {
                client.BaseAddress = new System.Uri (BASE_URL);
                HttpResponseMessage response = await client.GetAsync (referenceUrl);
                if (response.IsSuccessStatusCode)
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<RecommendationResponse> (await response.Content.ReadAsStringAsync ());
                    return deserializedResponse.results;
                }
            //}

            return null;
        }
    }
}