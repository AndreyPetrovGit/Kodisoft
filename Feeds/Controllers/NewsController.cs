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

            var list2 = _db.Feeds.Include(f => f.NewItems).ToList();
            var list3 = list2.FirstOrDefault(f => f.Id == id);
            List<NewsItem> newsCopy = new List<NewsItem>();
            var res = list3.NewItems.ToList();
            res.ForEach(n => newsCopy.Add(new NewsItem() {Id = n.Id, FeedId = n.FeedId, Link = n.Link, Title = n.Title, Description = n.Description }));
            return newsCopy;
        }
        [HttpGet("Collection/{id}")]
        [Authorize]
        public IEnumerable<NewsItem> GetCollection(int id)
        {
            return new List<NewsItem>();
        }
        public NewsController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
