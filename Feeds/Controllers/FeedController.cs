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
namespace Feeds.Controllers

{
    //https://feedly.com/i/subscription/feed/http://www.engadget.com/rss-full.xml
    [Route("api/FeedController")] //http://localhost:50515/api/FeedController
    public class FeedController : Controller
    {
        private FeedDbContext _db;
        [HttpGet]
        [Authorize]
        public IEnumerable<Feed> Get()
        {
            List<String> allAnswer = new List<string>();
            //http://feeds.feedburner.com/PhilosophicalGeek -ok
            //http://fooblog.com/feed - error
            // https://www.theverge.com/rss/index.xml  -atom ?
            // https://www.engadget.com/rss.xml - good but addition content https://www.engadget.com/rss.xml
            // Collection collection = new Collection() { Id = 0, Name = "First Collection", Feeds = new List<Feed>() };
            IEnumerable<Feed> feeds = _db.Feeds;
            return feeds;
        }

        // 0@
        [HttpGet("{id}")]
        [Authorize]
        public string Get(int id)
        {
            var user =  User.Identity.Name ;
            Feed feed = _db.Feeds.FirstOrDefault(f => f.Id == id);
            return JsonConvert.SerializeObject(feed);
        }
        // 2@
        [HttpGet("GetFromCollection/{id}")]
        [Authorize]
        public IEnumerable<Feed> GetFromCollection(int id)
        {
            var user = User.Identity.Name;
            ICollection<FeedCollection> feedsCollection = _db.Collections.FirstOrDefault(f => f.Id == id )
                            ?.FeedCollections;
            IEnumerable<Feed> res = new List<Feed>();
            if (feedsCollection != null) { 
                res = from t1 in feedsCollection
                          join t2 in _db.Feeds on t1.FeedId equals t2.Id
                          select t2;
            }
            return res;
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        [RouteAttribute("post")]
        public void Post([FromBody]string value)
        {
            _db.Feeds.Add(new Feed(value, _db));
            _db.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpGet("Delete/{id}")]
        [Authorize]
        public  void Delete(int id)
        {

                Feed feed =  _db.Feeds.ToList().FirstOrDefault(f => f.Id == id);
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
