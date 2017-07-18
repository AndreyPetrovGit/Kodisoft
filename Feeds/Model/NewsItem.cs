using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
namespace Feeds.Model
{
    public class NewsItem
    {

        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Link { get; set; }
        public String Description { get; set; }
        public Int32 FeedId { get; set; }
        public Feed Feed { get; set; }
        public NewsItem()
        {

        }
    }
}
