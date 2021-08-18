﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniCRMCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MiniCRMCore.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MiniCRMCore.Areas.Auth.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<Guid>("Salt")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Clients.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Contact")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Diagnostics")
                        .HasColumnType("text");

                    b.Property<string>("DomainNames")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("LegalEntitiesNames")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Clients.Models.ClientCommunicationReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientCommunicationReports");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Common.FileDatum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FileData");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("BriefIndustryDescription")
                        .HasColumnType("text");

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<Guid>("ClientLink")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CoveringLetter")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("CurrentVersion")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("NewsLinks")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<string>("OfferCase")
                        .HasColumnType("text");

                    b.Property<string>("OfferPoint")
                        .HasColumnType("text");

                    b.Property<string>("OtherDocumentation")
                        .HasColumnType("text");

                    b.Property<string>("Potential")
                        .HasColumnType("text");

                    b.Property<string>("ProductSystemType")
                        .HasColumnType("text");

                    b.Property<string>("Recommendations")
                        .HasColumnType("text");

                    b.Property<List<string>>("SelectedSections")
                        .HasColumnType("text[]");

                    b.Property<string>("SimilarCases")
                        .HasColumnType("text");

                    b.Property<string>("Stage")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferFileDatum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("FileDatumId")
                        .HasColumnType("integer");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FileDatumId")
                        .IsUnique();

                    b.HasIndex("OfferId");

                    b.ToTable("OfferFileData");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferNewsbreak", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferNewsbreaks");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferVersions");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Clients.Models.ClientCommunicationReport", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Clients.Models.Client", "Client")
                        .WithMany("CommunicationReports")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.Offer", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Clients.Models.Client", "Client")
                        .WithMany("Offers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferFileDatum", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Common.FileDatum", "FileDatum")
                        .WithOne()
                        .HasForeignKey("MiniCRMCore.Areas.Offers.Models.OfferFileDatum", "FileDatumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", "Offer")
                        .WithMany("FileData")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileDatum");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferNewsbreak", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", "Offer")
                        .WithMany("Newsbreaks")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferVersion", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", "Offer")
                        .WithMany("Versions")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Clients.Models.Client", b =>
                {
                    b.Navigation("CommunicationReports");

                    b.Navigation("Offers");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.Offer", b =>
                {
                    b.Navigation("FileData");

                    b.Navigation("Newsbreaks");

                    b.Navigation("Versions");
                });
#pragma warning restore 612, 618
        }
    }
}
