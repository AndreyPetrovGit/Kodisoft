using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Feeds.DAL;
using Feeds.Services;
using Feeds.Services.FeedProvider;
using System.Diagnostics;
using System.Globalization;
//using Feeds.Model;

namespace Feeds.Services.Jobs
{
    public class FeedDbUpdater: IJob
    {
        private static FeedDbContext _db;
        public Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"Quartz: update db.{DateTime.Now.ToString(new CultureInfo("en"))}  -   {context.JobDetail.Key}");
           /* foreach(Feed feed in _db.Feeds)
            {
                var feedProvider = new Services.FeedProvider.FeedProvider(_db);
                var newFeedData = feedProvider.GetFeedFromHttp(feed.Link);
                feed.Title = newFeedData.Title;
                feed.Descripton = newFeedData.Descripton;
                feed.NewItems = newFeedData.NewItems;

            }
            _db.SaveChanges();*/
            return Task.FromResult(0);
        }
        public FeedDbUpdater(FeedDbContext db)
        {
            _db = db;
        }
    }
}
