using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Feeds.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Feeds.DAL;
using Feeds.Services.Jobs;
using Feeds.Services.FeedProvider;
using System.Threading;

namespace Feeds.Controllers

{
    //https://feedly.com/i/subscription/feed/http://www.engadget.com/rss-full.xml
    [Route("api/FeedController")] //http://localhost:50515/api/FeedController
    public class FeedController : Controller
    {
        private FeedDbContext _db;

        /// <summary>
        ///  Return all feeds, client method #1
        /// </summary>
        /// <returns>IEnumerable<Feed></returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<Feed> Get()
        {
            IEnumerable<Feed> feeds = _db.Feeds;
            return feeds;
        }

        /// <summary>
        /// Return feed by id, client method #0 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Feed Get(int id)
        {
            Feed feed = _db.Feeds.FirstOrDefault(f => f.Id == id);
            return feed;
        }

        [HttpGet("GetFromCollection/{id}")]
        [Authorize]
        public IEnumerable<Feed> GetFromCollection(int id)
        {
            List<Feed> feeds = new List<Feed>();
            var collection = _db.Collections.Include(c => c.FeedCollections).ThenInclude(sc => sc.Feed).ToList().FirstOrDefault(e => e.Id == id);
            if(collection != null && collection.FeedCollections != null)
            {
                var hardFeed = collection.FeedCollections;
                hardFeed.Select(sc => sc.Feed).ToList()
                .ForEach(i => feeds
                       .Add(new Feed()
                        {
                            Id = i.Id,
                            Link = i.Link,
                            Descripton = i.Descripton,
                            Title = i.Title,
                            NewItems = i.NewItems
                        }));
            }
            return feeds;
        }
        /// <summary>
        /// Save new feed.
        /// </summary>
        /// <param name="url"></param>
        [HttpPost]
        [Authorize]
        [RouteAttribute("post")]
        public void Post([FromBody]string url)
        {
            var feedProvider = new FeedProvider(_db);
            Feed newFeed = feedProvider.GetFeed(url);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpGet("Delete/{cid}/{id}")]
        [Authorize]
        public  void Delete(int cid, int id)
        {

            var collection = _db.Collections.Include(c => c.FeedCollections).ThenInclude(sc => sc.Feed)
                                .ToList().FirstOrDefault(e => e.Id == cid);
            var feedCollection = collection?.FeedCollections.FirstOrDefault(f => f.FeedId == id);
            if (feedCollection != null)
            {
                collection.FeedCollections.Remove(feedCollection);
                _db.SaveChanges();
            }
        }

        [HttpGet("Delete/{id}")]
        [Authorize]
        public void Delete(int id)
        {
            Feed feed = _db.Feeds.ToList().FirstOrDefault(f => f.Id == id);
            if (feed != null)
            {
                _db.Feeds.Remove(feed);
                _db.SaveChanges();
            }
        }
        public FeedController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
