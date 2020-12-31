using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Series_watcher.Migrations
{
    public partial class AirDate_adddedTOEpisode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AirDateNewEpisode",
                table: "TvseriesListings",
                newName: "NewEpisodeAirDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEpisodeAirDate",
                table: "TvseriesListings",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEpisodeAirDate",
                table: "TvseriesListings");

            migrationBuilder.RenameColumn(
                name: "NewEpisodeAirDate",
                table: "TvseriesListings",
                newName: "AirDateNewEpisode");
        }
    }
}
