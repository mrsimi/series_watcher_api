using Microsoft.EntityFrameworkCore.Migrations;

namespace Series_watcher.Migrations
{
    public partial class LastSeenEpisode_To_LastEpisode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSeenEpisode",
                table: "TvseriesListings",
                newName: "LastEpisode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastEpisode",
                table: "TvseriesListings",
                newName: "LastSeenEpisode");
        }
    }
}
