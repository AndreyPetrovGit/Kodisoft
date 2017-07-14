using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
namespace FeedsClient.Model
{
    public class Feed
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Descripton { get; set; }
        public String Link { get; set; }
        public ICollection<NewsItem> NewItems { get; set; }
        public override string ToString()
        {
            return $"Id:{Id}    Title:{Title}     Link:{Link}    \nDescripton:{Descripton} ";
        }
    }
}
