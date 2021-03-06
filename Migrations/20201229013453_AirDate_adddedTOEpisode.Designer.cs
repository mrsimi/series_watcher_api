﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Series_watcher.Data;

namespace Series_watcher.Migrations
{
    [DbContext(typeof(SeriesDbContext))]
    [Migration("20201229013453_AirDate_adddedTOEpisode")]
    partial class AirDate_adddedTOEpisode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Series_watcher.Models.TvSeriesListing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentSeason")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExternalSeriesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LastEpisode")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastEpisodeAirDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("NewEpisode")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("NewEpisodeAirDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SeriesTitle")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TvseriesListings");
                });

            modelBuilder.Entity("Series_watcher.Models.TvSeriesRecommendation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SeriesTitle")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TvSeriesRecommendations");
                });
#pragma warning restore 612, 618
        }
    }
}
