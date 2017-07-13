using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using JetBrains.Annotations;
namespace Feeds.Model
{
    public class FeedDbContext: DbContext
    {
        public FeedDbContext(DbContextOptions<FeedDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Feed> Feeds { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedCollection>()
                .HasKey(t => new { t.CollectionId, t.FeedId});

            modelBuilder.Entity<FeedCollection>()
                .HasOne(pt => pt.Collection)
                .WithMany(p => p.FeedCollections)
                .HasForeignKey(pt => pt.CollectionId);

            modelBuilder.Entity<FeedCollection>()
                .HasOne(pt => pt.Feed)
                .WithMany(t => t.FeedCollections)
                .HasForeignKey(pt => pt.FeedId);
        }
    }
}
