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
namespace Feeds.Controllers
{
    [Route("api/News")]
    public class NewsController : Controller
    {
        private FeedDbContext _db;
        [HttpGet("{id}")]
        [Authorize]
        public IEnumerable<NewsItem> Get(int id)
        {
            List<NewsItem> newsCopy = new List<NewsItem>();
            var feed = _db.Feeds.Include(f => f.NewItems).ToList().FirstOrDefault(f => f.Id == id);
            if(feed != null)
            {
                var news = feed.NewItems.ToList();
                news.ForEach(n => newsCopy.Add(new NewsItem() { Id = n.Id, FeedId = n.FeedId, Link = n.Link, Title = n.Title, Description = n.Description }));
            }
            return newsCopy;
        }
        [HttpGet("Collection/{id}")]
        [Authorize]
        public IEnumerable<NewsItem> GetCollection(int id)
        {
            List<NewsItem> newsCopy = new List<NewsItem>();
            var collection = _db.Collections.Include(c => c.FeedCollections)
                .ThenInclude(sc => sc.Feed).ThenInclude(n=>n.NewItems).ToList().FirstOrDefault(f => f.Id == id);
            var feedCollection = collection.FeedCollections;
            feedCollection.Select(sc => sc.Feed).ToList()
            .ForEach(feed => {
                if (feed != null)
                {
                    var news = feed?.NewItems;
                    if (news != null)
                    {
                        news.ToList().ForEach(n => newsCopy.Add(new NewsItem() { Id = n.Id, FeedId = n.FeedId, Link = n.Link, Title = n.Title, Description = n.Description }));
                    }
                }
            });
            return newsCopy;
        }
        public NewsController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
