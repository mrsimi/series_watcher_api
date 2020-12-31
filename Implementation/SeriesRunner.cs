using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Series_watcher.Data;
using Series_watcher.DTO;
using Series_watcher.Interfaces;
using Series_watcher.Models;

namespace Series_watcher.Implementation
{
    public class SeriesRunner : ISeriesRunner
    {
        private readonly SeriesDbContext _context;
        private readonly ISeriesQuerier _seriesQuerier;
        public SeriesRunner (SeriesDbContext context, ISeriesQuerier seriesQuerier)
        {
            _context = context;
            _seriesQuerier = seriesQuerier;

            var tvlisting = new List<TvSeriesListing> ()
            {
                new TvSeriesListing
                {
                SeriesTitle = "Seal Team",
                LastEpisode = 4,
                CurrentSeason = 4,
                },
                new TvSeriesListing
                {
                SeriesTitle = "The Rookie",
                LastEpisode = 1,
                CurrentSeason = 3
                },
                new TvSeriesListing
                {
                SeriesTitle = "The Good Doctor",
                LastEpisode = 5,
                CurrentSeason = 4
                },
                new TvSeriesListing
                {
                SeriesTitle = "Lucifer",
                LastEpisode = 5,
                CurrentSeason = 5
                },
                new TvSeriesListing
                {
                SeriesTitle = "New Amsterdam",
                LastEpisode = 18,
                CurrentSeason = 2
                },
                new TvSeriesListing
                {
                SeriesTitle = "The Blacklist",
                LastEpisode = 2,
                CurrentSeason = 8
                },
                new TvSeriesListing
                {
                SeriesTitle = "Private Eyes",
                LastEpisode = 5,
                CurrentSeason = 4
                },
                new TvSeriesListing
                {
                SeriesTitle = "Chicago Med",
                LastEpisode = 2,
                CurrentSeason = 6
                },
                new TvSeriesListing
                {
                    SeriesTitle = "MacGyver",
                    LastEpisode = 2,
                    CurrentSeason = 6
                },
                new TvSeriesListing
                {
                    SeriesTitle = "Magnum P.I.",
                    LastEpisode = 3,
                    CurrentSeason = 3
                },
            };

            var alltv = from c in _context.TvseriesListings select c;

            if (tvlisting.Count != alltv.Count ())
            {
                _context.RemoveRange (alltv);
                _context.AddRange (tvlisting);
                _context.SaveChanges ();
            }
        }
        public async Task<List<TvSeriesDTO>> GetAvailiabilityStatus ()
        {
            var ids = await GetExternalSeriesId ();

            var allSeries = _context.TvseriesListings.ToList ();
            List<TvSeriesDTO> seriesDTOs = new List<TvSeriesDTO> ();

            foreach (var series in allSeries)
            {
                if (series.ExternalSeriesId != 0)
                {
                    int days = 0;
                    try
                    {
                        days = series.NewEpisodeAirDate.Value.Subtract (DateTime.Now).Days;
                    }
                    catch (InvalidOperationException)
                    {
                        days = 0;
                    }
                    // if (days <= 0)
                    //{
                    if (series.NewEpisode != series.LastEpisode)
                    {
                        var details = await _seriesQuerier.GetLastTwoEpisodes (series.ExternalSeriesId, series.CurrentSeason);
                        if (details.Count != 0)
                        {
                            if (details.Count == 1)
                            {
                                series.LastEpisode = details[0].episode_number;
                                series.NewEpisode = details[0].episode_number;
                                series.LastEpisodeAirDate = string.IsNullOrEmpty (details[0].air_date) ? DateTime.Now.AddDays (20) : DateTime.Parse (details[0].air_date);

                                _context.Update (series);

                                var seriesToSave = new TvSeriesDTO
                                {
                                    SeriesTitle = series.SeriesTitle,
                                    CurrentSeason = series.CurrentSeason,
                                    LastEpisode = series.LastEpisode,
                                    NewEpisode = series.NewEpisode,
                                    LastEpisodeAirDate = series.LastEpisodeAirDate,
                                    NewEpisodeAirDate = series.NewEpisodeAirDate,
                                };
                                
                                if(seriesToSave.LastEpisodeAirDate.Value.Subtract(DateTime.Now).Days <= 0)
                                {
                                    seriesToSave.HasCurrentSeasonEnded = true;
                                }
                               

                                seriesDTOs.Add (seriesToSave);
                            }
                            else
                            {
                                series.LastEpisode = details[0].episode_number;
                                series.NewEpisode = details[1].episode_number;
                                series.LastEpisodeAirDate = string.IsNullOrEmpty (details[0].air_date) ? DateTime.Now.AddDays (20)  : DateTime.Parse (details[0].air_date);
                                series.NewEpisodeAirDate = string.IsNullOrEmpty (details[1].air_date) ? DateTime.Now.AddDays (40) : DateTime.Parse (details[1].air_date);

                                _context.Update (series);

                                var seriesToSave = new TvSeriesDTO
                                {
                                    SeriesTitle = series.SeriesTitle,
                                    CurrentSeason = series.CurrentSeason,
                                    LastEpisode = series.LastEpisode,
                                    NewEpisode = series.NewEpisode,
                                    LastEpisodeAirDate = series.LastEpisodeAirDate,
                                    NewEpisodeAirDate = series.NewEpisodeAirDate,
                                };

                                seriesDTOs.Add (seriesToSave);
                            }
                        }
                    }
                    //}
                }
            }

            _context.SaveChanges ();

            return seriesDTOs;
        }

        public async Task<List<TvSeriesListing>> GetExternalSeriesId ()
        {
            var allSeries = _context.TvseriesListings.ToList ();

            foreach (var series in allSeries)
            {
                if (series.ExternalSeriesId == 0)
                {
                    int externalId = await _seriesQuerier.GetSeriesExternalId (series.SeriesTitle);
                    if (externalId != 0)
                    {
                        series.ExternalSeriesId = externalId;
                        // _context.Update(series);
                    }
                }
            }

            await _context.SaveChangesAsync ();

            return allSeries;
        }

        public async Task<List<SeriesRecommendation>> GetRecommendations ()
        {
            List<SeriesRecommendation> seriesRecommendations = new List<SeriesRecommendation> ();
            var seriesList = await _seriesQuerier.GetTvShowsRecommendations ();
            if (seriesList != null)
            {
                var previousRecommendations = _context.TvSeriesRecommendations.Select (m => m.SeriesTitle).ToList ();
                foreach (var series in seriesList)
                {
                    if (!previousRecommendations.Contains (series.name))
                    {
                        seriesRecommendations.Add (new SeriesRecommendation
                        {
                            Name = series.name,
                                Date = series.first_air_date,
                                Overview = series.overview,
                                Picture = series.backdrop_path != null? series.backdrop_path : series.poster_path
                        });

                        _context.TvSeriesRecommendations.Add (new TvSeriesRecommendation
                        {
                            SeriesTitle = series.name
                        });
                    }

                    _context.SaveChanges ();

                    if (seriesRecommendations.Count == 5)
                    {
                        return seriesRecommendations;
                    }
                }

                if (seriesRecommendations.Count == 0)
                {
                    await GetRecommendations ();
                }

            }

            return seriesRecommendations;
        }
    }
}