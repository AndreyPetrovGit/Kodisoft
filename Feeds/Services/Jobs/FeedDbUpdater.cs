using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Feeds.DAL;
using Feeds.Services;
using System.Diagnostics;
using System.Globalization;
namespace Feeds.Services.Jobs
{
    public class FeedDbUpdater: IJob
    {
        private static FeedDbContext _db;
        public Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"Quartz: update db.{DateTime.Now.ToString(new CultureInfo("en"))}  -   {context.JobDetail.Key}");
            return Task.FromResult(0);
        }
       /* public FeedDbUpdater(FeedDbContext db)
        {
            _db = db;
        }*/
    }
}
