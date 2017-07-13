using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Model
{
    public class Collection
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public User User { get; set; }
        public String Name { get; set; }
        public ICollection<FeedCollection> FeedCollections { get; set; }
        public Collection()
        {
            FeedCollections = new List<FeedCollection>();
        }
    }
}
