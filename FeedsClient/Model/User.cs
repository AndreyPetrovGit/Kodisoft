using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedsClient.Model
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Login { get;set;}
        public String Password { get; set; }
        public ICollection<Collection> Collections { get; set; }
        public User()
        {
            Collections = new List<Collection>();
        }
    }
}
