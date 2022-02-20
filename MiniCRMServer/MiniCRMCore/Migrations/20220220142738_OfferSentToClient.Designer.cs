﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniCRMCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MiniCRMCore.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220220142738_OfferSentToClient")]
    partial class OfferSentToClient
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("AllowedToViewAllOffers")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("MiniCRMCore.Areas.Common.CommunicationReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("OfferId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ClientId");

                    b.HasIndex("OfferId");

                    b.ToTable("CommunicationReports");
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

            modelBuilder.Entity("MiniCRMCore.Areas.Email.Models.EmailSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<string>("SenderEmail")
                        .HasColumnType("text");

                    b.Property<string>("SenderName")
                        .HasColumnType("text");

                    b.Property<string>("SmtpHost")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EmailSettings");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Logs.LogEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FullMessage")
                        .HasColumnType("text");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Scope")
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("LogEntries");
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
                        .HasColumnType("uuid");

                    b.Property<int>("ClientVersionNumber")
                        .HasColumnType("integer");

                    b.Property<string>("CoveringLetter")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("CurrentVersionNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

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

                    b.HasIndex("ManagerId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferFeedbackRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AnswerText")
                        .HasColumnType("text");

                    b.Property<bool>("Answered")
                        .HasColumnType("boolean");

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

                    b.ToTable("OfferFeedbackRequests");
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

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Changed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<string>("Report")
                        .HasColumnType("text");

                    b.Property<string>("Task")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferRules");
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

                    b.Property<bool>("SentToClient")
                        .HasColumnType("boolean");

                    b.Property<bool>("VisitedByClient")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferVersions");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Common.CommunicationReport", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Clients.Models.Client", null)
                        .WithMany("CommonCommunicationReports")
                        .HasForeignKey("ClientId");

                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", null)
                        .WithMany("CommonCommunicationReports")
                        .HasForeignKey("OfferId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.Offer", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Clients.Models.Client", "Client")
                        .WithMany("Offers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferFeedbackRequest", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Auth.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", "Offer")
                        .WithMany("FeedbackRequests")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Offer");
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

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.OfferRule", b =>
                {
                    b.HasOne("MiniCRMCore.Areas.Offers.Models.Offer", "Offer")
                        .WithMany("Rules")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                    b.Navigation("CommonCommunicationReports");

                    b.Navigation("Offers");
                });

            modelBuilder.Entity("MiniCRMCore.Areas.Offers.Models.Offer", b =>
                {
                    b.Navigation("CommonCommunicationReports");

                    b.Navigation("FeedbackRequests");

                    b.Navigation("FileData");

                    b.Navigation("Newsbreaks");

                    b.Navigation("Rules");

                    b.Navigation("Versions");
                });
#pragma warning restore 612, 618
        }
    }
}
