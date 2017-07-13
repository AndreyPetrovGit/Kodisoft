using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;


namespace FeedsClient
{
  /*  public class FeedClient
    {
        public String BaseUrl { get; set; }
        public String Token { get; set; }
        public async Task<String> GetToken()
        {
            Console.WriteLine("Enter login:");
            var username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            var password = Console.ReadLine();
            var grant_type = "password";
            String jsonInString = $"{{\"grant_type\":\"{grant_type}\", \"username\": \"{username}\", \"password\": \"{password}\"}}";
            using (var client = new HttpClient())
            {
                using (var r = await client.PostAsync(BaseUrl + "/AuthenticationController/token", new StringContent(jsonInString, Encoding.UTF8, "application/json")))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    return result;
                }
            }
        }
        public async void ShowContent(string controller)
        {
            var r = await GetContent(BaseUrl +"/"+controller);
            Console.WriteLine(r);
        }

        public async Task<String> GetContent(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                using (var r = await client.GetAsync(new Uri(url)))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    return result;
                }
            }
        }
        public async void PostFeed(String feedUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                String jsonInString = JsonConvert.SerializeObject(feedUrl, new JsonSerializerSettings { Formatting = Formatting.Indented });;
                Console.WriteLine();
                await client.PostAsync(BaseUrl + "/FeedController/post", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            }
        }

        public FeedClient(String url)
        {
            BaseUrl = url;
        }
    }
    */
}
