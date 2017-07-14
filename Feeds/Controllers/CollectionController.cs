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
    [Route("api/Collection")]
    public class CollectionController: Controller
    {
        private FeedDbContext _db;

        [HttpGet]
        [Authorize]
        public IEnumerable<Collection> Get()
        {           
            return _db.Collections.Where(c => c.User.Login == User.Identity.Name);
        }

        [HttpGet("{id}")]
        [Authorize]
        public string Get(int id)
        {
            Collection collection = _db.Collections.FirstOrDefault(f => f.Id == id);
            return JsonConvert.SerializeObject(collection);
        }

        [HttpPost]
        [Authorize]
        [RouteAttribute("create")]
        public Int32 Post([FromBody]string value)
        {
            var newCol = new Collection()
            {
                Name = value,
                User = _db.Users.First(u => u.Login == User.Identity.Name)
            };
            _db.Collections.Add(newCol);
            _db.SaveChanges();
            return _db.Collections.First(c => c.Name == value 
            && c.User == _db.Users.First(u => u.Login == User.Identity.Name)).Id;
        }
        
        /// <summary>
        /// Add Feed to Collection, client function # 13
        /// </summary>
        /// <param name="id"></param>
        /// <param name="feedId"></param>
        [HttpPut("add/{id}")]
        [Authorize]
        public void Put(int id, [FromBody]int feedId)
        {
            Collection selectedCollection = _db.Collections.First(c => c.Id == id );
            FeedCollection feedCollection = new FeedCollection()
            {
                CollectionId = id,
                FeedId = feedId
            };
            selectedCollection.FeedCollections.Add(feedCollection);
            _db.SaveChanges();
        }
        public CollectionController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
