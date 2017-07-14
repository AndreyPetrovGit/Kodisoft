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

        // 2@
        [HttpGet("GetFromCollection/{id}")]
        [Authorize]
        public IEnumerable<Feed> GetFromCollection(int id)
        {

            List<Feed> feeds = new List<Feed>();
            var collections = _db.Collections.Include(c => c.FeedCollections).ThenInclude(sc => sc.Feed).ToList();
            foreach (var collection in collections)
            {
                if(collection.Id == id)
                {
                    var ir = collection.FeedCollections;
                    ir.Select(sc => sc.Feed).ToList()
                    .ForEach( i=> feeds
                            .Add(new Feed()
                            {
                                Id = i.Id,
                                Link = i.Link,
                                Descripton = i.Descripton,
                                Title = i.Title,
                                NewItems = i.NewItems
                            }));
                }
            }
            return feeds;
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        [RouteAttribute("post")]
        public void Post([FromBody]string value)
        {
            var newFeed = new Feed(value, _db);
            foreach (var item in newFeed.NewItems)
            {
                item.FeedId = newFeed.Id;
            }
            _db.Feeds.Add(newFeed);
           _db.NewsItems.AddRange(newFeed.NewItems);
            _db.SaveChanges();
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

            Collection selectedCollection = _db.Collections.First(c => c.Id == cid);
            FeedCollection fc = selectedCollection.FeedCollections.FirstOrDefault(c => c.FeedId == id && c.CollectionId == cid);
            if (fc != null)
            {
                selectedCollection.FeedCollections.Remove(fc);
            }
            _db.SaveChanges();
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
