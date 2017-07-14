using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
namespace Feeds.Model
{
    public class Feed
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Descripton { get; set; }
        public String Link { get; set; }

        public ICollection<NewsItem> NewItems { get; set; }
        public ICollection<FeedCollection> FeedCollections { get; set; }

        public override string ToString()
        {
            return string.Format($"Id={Id}, Title={Title}, Link={Link}, \nDescription={Descripton}");
        }
        public bool Contains(NewsItem item)
        {
            foreach (NewsItem itemForCheck in NewItems)
            {
                if(item.Title == itemForCheck.Title)
                {
                    return true;
                }
            }
            return false;
        }
        public NewsItem GetItem(String title)
        {
            foreach (NewsItem itemForCheck in NewItems)
            {        
                if(itemForCheck.Title == title)
                {
                    return itemForCheck;
                }
            }
            return null;
        }
        public Feed()
        {

        }
        public Feed(String url, FeedDbContext db)
        {
            NewItems = new List<NewsItem>();
            FeedCollections = new List<FeedCollection>();
            XmlTextReader xmlTextReader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlTextReader);
                xmlTextReader.Close();
                XmlNode channelXmlNode = xmlDoc.GetElementsByTagName("channel")[0];
                if(channelXmlNode != null)
                {
                    foreach(XmlNode channelNode in channelXmlNode.ChildNodes)
                    {
                        switch (channelNode.Name)
                        {
                            case "title":
                                {
                                    Title = channelNode.InnerText;
                                    break;
                                }
                            case "description":
                                {
                                    Descripton = channelNode.InnerText;
                                    break;
                                }
                            case "link":
                                {
                                    Link = channelNode.InnerText;
                                    break;
                                }
                            case "item":
                                {
                                    NewsItem channelItem = new NewsItem(channelNode, this);
                                    db.NewsItems.Add(channelItem);
                                    
                                    NewItems.Add(channelItem);
                                    db.SaveChanges();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                } else
                {
                    throw new Exception("XML error. Chanels description not found!");
                }
            } catch(System.Net.WebException ex)
            {
                if (ex.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
                {
                    throw new Exception("Connection with this source is impossible! " + url);
                } else {
                    throw ex;
                    }
            } catch (System.IO.FileNotFoundException)
            {
                throw new Exception("File " + url + " not found!");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            finally
            {
                xmlTextReader.Close();
            } 
        }
    }
}
