using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Series_watcher.Migrations
{
    public partial class InitialDatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TvseriesListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeriesTitle = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentSeason = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalSeriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastSeenEpisode = table.Column<int>(type: "INTEGER", nullable: false),
                    NewEpisode = table.Column<int>(type: "INTEGER", nullable: false),
                    AirDateNewEpisode = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvseriesListings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TvSeriesRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeriesTitle = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvSeriesRecommendations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TvseriesListings");

            migrationBuilder.DropTable(
                name: "TvSeriesRecommendations");
        }
    }
}
