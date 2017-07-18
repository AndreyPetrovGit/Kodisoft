using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feeds.Model;
using Feeds.DAL;
namespace Feeds.Services.FeedWriter
{
    public class FeedWriter
    {
        private FeedDbContext _db;
        public void AddToDB(Feed feed)
        {
            _db.Feeds.Add(feed);
            _db.SaveChanges();
        }
        public FeedWriter(FeedDbContext db)
        {
            _db = db;
        }
    }
}
