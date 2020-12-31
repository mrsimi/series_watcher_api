using Microsoft.EntityFrameworkCore;
using Series_watcher.Models;

namespace Series_watcher.Data
{
    public class SeriesDbContext : DbContext
    {
        public SeriesDbContext(DbContextOptions<SeriesDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<TvSeriesListing> TvseriesListings { get; set; }
        public DbSet<TvSeriesRecommendation> TvSeriesRecommendations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // builder.UseSqlite(@"Data source=tvseries.db");
        }
    }
}