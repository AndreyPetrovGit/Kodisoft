using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Model
{
    public class FeedCollection
    {

        public Int32 FeedId { get; set; }
        public Feed Feed { get; set; }
        public Int32 CollectionId { get; set; }
        public Collection Collection { get; set; }
    }
}
