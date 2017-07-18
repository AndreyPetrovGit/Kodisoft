using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feeds.Model;
using System.Xml;
namespace Feeds.Services.NewItemProviders
{
    public class NewsItemXMLProvider
    {
        public NewsItem  GetWewsItem(XmlNode xmlNode)
        {
            String title = default(string);
            String description = default(string);
            String link = default(string);
            NewsItem newsItem;
            foreach (XmlNode xmlTag in xmlNode.ChildNodes)
            {
                switch (xmlTag.Name)
                {
                    case "title":
                        {
                            title = xmlTag.InnerText;
                            break;
                        }
                    case "description":
                        {
                            description = xmlTag.InnerText;
                            break;
                        }
                    case "link":
                        {
                            link = xmlTag.InnerText;
                            break;
                        }
                    default:
                        break;
                }
            }
            newsItem = new NewsItem()
            {
                Title = title,
                Description = description,
                Link = link
            };
            return newsItem;
        }
    }
}
