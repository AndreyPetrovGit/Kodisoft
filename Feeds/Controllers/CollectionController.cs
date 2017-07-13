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
        //  @13    
        // PUT api/values/5
        [HttpPut("add/{id}")]
        [Authorize]
        public void Put(int id, [FromBody]int feedId)
        {
            FeedCollection feedCollection = new FeedCollection()
            {
                CollectionId = id,
                Collection = _db.Collections.First(c => c.Id == id),
                FeedId = feedId,
                Feed = _db.Feeds.First(f => f.Id == feedId)
            };
            if(feedCollection != null)
            { 
                _db.Collections.First(c => c.Id == id).FeedCollections.Add( feedCollection);
                _db.Feeds.First(f => f.Id == feedId).FeedCollections.Add(feedCollection);
            }
        }
        public CollectionController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
