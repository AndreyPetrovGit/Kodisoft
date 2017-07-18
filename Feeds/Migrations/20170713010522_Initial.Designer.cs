using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
//using Feeds.Model;
using Feeds.DAL;
namespace Feeds.Migrations
{
    [DbContext(typeof(FeedDbContext))]
    [Migration("20170713010522_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Feeds.Model.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Feeds.Model.Feed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripton");

                    b.Property<string>("Link");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("Feeds.Model.FeedCollection", b =>
                {
                    b.Property<int>("CollectionId");

                    b.Property<int>("FeedId");

                    b.HasKey("CollectionId", "FeedId");

                    b.HasIndex("FeedId");

                    b.ToTable("FeedCollection");
                });

            modelBuilder.Entity("Feeds.Model.NewsItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("FeedId");

                    b.Property<string>("Link");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.ToTable("NewsItems");
                });

            modelBuilder.Entity("Feeds.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Feeds.Model.Collection", b =>
                {
                    b.HasOne("Feeds.Model.User", "User")
                        .WithMany("Collections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Feeds.Model.FeedCollection", b =>
                {
                    b.HasOne("Feeds.Model.Collection", "Collection")
                        .WithMany("FeedCollections")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Feeds.Model.Feed", "Feed")
                        .WithMany("FeedCollections")
                        .HasForeignKey("FeedId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Feeds.Model.NewsItem", b =>
                {
                    b.HasOne("Feeds.Model.Feed", "Feed")
                        .WithMany("NewItems")
                        .HasForeignKey("FeedId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
