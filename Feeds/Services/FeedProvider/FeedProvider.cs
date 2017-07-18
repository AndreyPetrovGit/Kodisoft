using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feeds.Model;
using Feeds.DAL;
using Feeds.Services;
using System.Xml;
using System.Xml.Linq;
using Feeds.Services.NewItemProviders;

namespace Feeds.Services.FeedProvider
{
    public  class FeedProvider
    {
        private FeedDbContext _db;
        public Feed GetFeed(string url)
        {
            Feed feed = GetFeedFromDb(url);


            if (feed == null)
            {
                feed = GetFeedFromHttp(url);
                FeedWriter.FeedWriter feedWriter = new FeedWriter.FeedWriter(_db);
                feedWriter.AddToDB(feed);
            }
            return feed;
        }
        private Feed GetFeedFromDb(string url)
        {
            return _db.Feeds.FirstOrDefault(f => f.Link == url);
        }
        private Feed GetFeedFromHttp(string url)
        {
            Feed feed = null;
            try
            {
                XDocument xmlDoc = XDocument.Load(url);
                feed = GetFeedFromXml(xmlDoc);
            }
            catch (Exception)
            {
                throw;
            }
            return feed;
        }
        private Feed GetFeedFromXml(XDocument xmlDoc)
        {
            Feed feed = null;
            String rootNodeName = xmlDoc.Root.Name.LocalName;
            switch (rootNodeName)
            {
                case "rss": feed = GetFeedFromRss2(xmlDoc); break;
                case "feed": feed = GetFeedFromAtom(xmlDoc); break;
                default: feed = null; break;
            }
            return feed;
        }
        private Feed GetFeedFromRss2(XDocument xmlDoc)
        {
            Feed feed = null;
            XElement channel = xmlDoc.Element("rss").Element("channel");
            var news = from item in channel.Elements("item")
                          select new NewsItem
                          {
                              Title = item.Element("title").Value,
                              Link = item.Element("link").Value,
                              Description = item.Element("description").Value
                          };
            feed = new Feed()
            {
                Title = channel.Element("title").Value,
                Link = channel.Element("link").Value,
                Descripton = channel.Element("description").Value,
                NewItems = news.ToList()
            };
            return feed;
            /* String title = default(string);
             String description = default(string);
             String link = default(string);
             NewsItem channelItem;
             List<NewsItem> newItems = new List<NewsItem>();
             Feed feed = null;
             NewsItemXMLProvider newItemXmlProvider = new NewsItemXMLProvider();
             try
             {
                 XmlNode channelXmlNode = xmlDoc.GetElementsByTagName("channel")[0];
                 if (channelXmlNode != null)
                 {
                     foreach (XmlNode channelNode in channelXmlNode.ChildNodes)
                     {
                         switch (channelNode.Name)
                         {
                             case "title":
                                 {
                                     title = channelNode.InnerText;
                                     break;
                                 }
                             case "description":
                                 {
                                     description = channelNode.InnerText;
                                     break;
                                 }
                             case "link":
                                 {
                                     link = channelNode.InnerText;
                                     break;
                                 }
                             case "item":
                                 {
                                     channelItem = newItemXmlProvider.GetWewsItem(channelNode);
                                     newItems.Add(channelItem);
                                     break;
                                 }
                             default:
                                 break;
                         }
                     }
                 }
                 else
                 {
                     throw new Exception("XML error. Chanels description not found!");
                 }
             }
             catch (Exception)
             {

                 throw;
             }
             feed = new Feed()
             {
                 Title = title,
                 Descripton = description,
                 Link = link,
                 NewItems = newItems
             };
             return feed;*/
        }
        private Feed GetFeedFromAtom(XDocument xmlDoc)
        {
            Feed feed = null;
            XElement feedNode = xmlDoc.Element("feed");
            var news = from item in feedNode.Elements("entry")
                       select new NewsItem
                       {
                           Title = item.Element("title").Value,
                           Link = item.Element("link").Value,
                           Description = item.Element("subtitle").Value
                       };
            feed = new Feed()
            {
                Title = feedNode.Element("title").Value,
                Link = feedNode.Element("link").Value,
                Descripton = $"{feedNode.Element("summary").Value}\n {feedNode.Element("content").Value}",
                NewItems = news.ToList()
            };
            return feed;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public FeedProvider(FeedDbContext db)
        {
            _db = db;
        }
    }
}
