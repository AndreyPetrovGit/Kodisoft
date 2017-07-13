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
    [Route("api/News")]
    public class NewsController : Controller
    {
        private FeedDbContext _db;
        [HttpGet("{id}")]
        [Authorize]
        public NewsItem Get(int id)
        {
            NewsItem news = _db.NewsItems.FirstOrDefault(f => f.Id == id);
            return news;
        }
        [HttpGet("Collection/{id}")]
        [Authorize]
        public IEnumerable<NewsItem> GetCollection(int id)
        {
            IEnumerable<NewsItem>  newsCollections = _db.Collections.First(c => c.Id == id)
                .FeedCollections.Select(a=>a.Feed).SelectMany(f=>f.NewItems);
            return newsCollections;
        }
        public NewsController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
