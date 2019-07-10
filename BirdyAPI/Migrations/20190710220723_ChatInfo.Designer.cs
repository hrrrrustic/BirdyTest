﻿// <auto-generated />
using System;
using BirdyAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BirdyAPI.Migrations
{
    [DbContext(typeof(BirdyContext))]
    [Migration("20190710220723_ChatInfo")]
    partial class ChatInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BirdyAPI.DataBaseModels.ChatInfo", b =>
                {
                    b.Property<Guid>("ChatID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatName");

                    b.HasKey("ChatID");

                    b.ToTable("ChatInfo");
                });

            modelBuilder.Entity("BirdyAPI.DataBaseModels.ChatUsers", b =>
                {
                    b.Property<Guid>("ChatID");

                    b.Property<int>("UserInChatID");

                    b.HasKey("ChatID", "UserInChatID");

                    b.ToTable("ChatUsers");
                });

            modelBuilder.Entity("BirdyAPI.DataBaseModels.Friend", b =>
                {
                    b.Property<int>("FirstUserID");

                    b.Property<int>("SecondUserID");

                    b.Property<bool>("RequestAccepted");

                    b.HasKey("FirstUserID", "SecondUserID");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("BirdyAPI.DataBaseModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AvatarReference");

                    b.Property<int>("CurrentStatus");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("PasswordHash");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BirdyAPI.DataBaseModels.UserSessions", b =>
                {
                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserId");

                    b.HasKey("Token");

                    b.ToTable("UserSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
