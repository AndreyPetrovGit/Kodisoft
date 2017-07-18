using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

using FeedsClient.Model;
namespace FeedsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new Manager();

            m.Run();
        }  
    }
}