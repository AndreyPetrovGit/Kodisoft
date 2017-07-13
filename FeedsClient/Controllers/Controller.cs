using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedsClient.View;

using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using FeedsClient.Model;

namespace FeedsClient.Controllers
{
    class Controller
    {
        #region Data
        SortedDictionary<UInt32, Action> _commandMap;   
        Viewer _view;

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
        #endregion
        #region Service commands

        private void ExecuteCommand(UInt32 commandId)
        {
            _commandMap[commandId]();
        }
        public void ManagerLoop()
        {
            Boolean _continue = true;
            UInt32 _commandId;
            while (_continue)
            {
                try { 
                _view.MenuRenderer();
                _commandId = DemandCommand();
                ExecuteCommand(_commandId);
                Console.ReadKey();

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Some Error");
                }
                finally{
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// User must enter some CommandId
        /// </summary>
        /// <returns></returns>
        private UInt32 DemandCommand()
        {
            UInt32 _commandId;
            Console.WriteLine("Enter command id and press Enter:");
            while (!UInt32.TryParse(Console.ReadLine(), out _commandId) || (!_commandMap.ContainsKey(_commandId))) ;
            return _commandId;
        }

        /// <summary>
        /// User must enter some Number
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Int32 DemandInt32(String message)
        {
            Int32 _commandId;
            Console.WriteLine(message);
            while (!Int32.TryParse(Console.ReadLine(), out _commandId)) ;
            return _commandId;
        }

        /// <summary>
        /// User must enter some String
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private String DemandString(String message)
        {
            _view.ShowMessage(message);
            String result = null;
            while (result == "" || result == null)
            {
                result = Console.ReadLine();
            }
            return result;
        }

        #endregion

        #region Main features
        //0 +
        public async void GetFeedById()
        {
            try { 
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    Int32 id = DemandInt32("Enter feed id:");
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/FeedController/{id}")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<Feed>(result);
                        Console.WriteLine($"Id:{objResponse.Id}    Title:{objResponse.Title}");
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }

        }
        //1 +
        public async void GetFeeds()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/FeedController")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<IEnumerable<Feed>>(result);
                        Console.WriteLine("Result:");
                        if (objResponse.Count() == 0)
                        {
                            Console.WriteLine("-");
                        }
                        foreach (var item in objResponse)
                        {
                            Console.WriteLine($"Id:{item.Id}    Title:{item.Title}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }
        }
        //2 +
        public async void GetFeedsByCollectionId()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    Int32 id = DemandInt32("Enter collection id:");
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/FeedController/GetFromCollection/{id}")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<IEnumerable<Feed>>(result);
                        Console.WriteLine("Result:");
                        if (objResponse.Count() == 0)
                        {
                            Console.WriteLine("-");
                        }
                        foreach (var item in objResponse)
                        {
                            Console.WriteLine($"Id:{item.Id}    Title:{item.Title}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }
        }
        //3 
        public async void DeleteFeedById()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Int32 feedId = DemandInt32("Enter feed id:");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/FeedController/Delete/{feedId}")))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }
        }
        //4 +
        public async void AddFeedWithRerf()
        {
            try
            {
                String feedUrl = DemandString("Enter feed url:");
                //String collectionId = DemandString("Enter collection id");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    String jsonInString = JsonConvert.SerializeObject(feedUrl, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    Console.WriteLine();
                    await client.PostAsync(BaseUrl + "/FeedController/post", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }

        }
        //5 +
        public async void GetMyCollections()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/Collection")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<IEnumerable<Collection>>(result);
                        Console.WriteLine("Result:");
                        if (objResponse.Count() == 0)
                        {
                            Console.WriteLine("-");
                        }
                        foreach (var item in objResponse)
                        {
                            Console.WriteLine($"Id:{item.Id}    Title:{item.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }

        }
        //6
        public async void GetCollectionById()
        {

        }
        //7
        public async void DeleteCollectionById()
        {

        }
        //8 +
        public async void CreateNewCollectionWithName()
        {
            try
            {
                String collectionName = DemandString("Enter collecion name:");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    String json = JsonConvert.SerializeObject(collectionName, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    Console.WriteLine();
                    var res = await client.PostAsync(BaseUrl + "/Collection/create", new StringContent(json, Encoding.UTF8, "application/json"));
                    string result = await res.Content.ReadAsStringAsync();
                    Console.WriteLine($"Collection created with Id = {result}");
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }
        }
        //9
        public async void GetNewsByFeedId()
        {
            try
            {
                Int32 newsItem = DemandInt32("Enter NewsItem Id:");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/News/{newsItem}")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<NewsItem>(result);
                        Console.WriteLine("Result:");
                        Console.WriteLine($"Id:{objResponse.Id}    Title:{objResponse.Title} Link:{objResponse.Link}");
                        Console.WriteLine($"Description:{objResponse.Description} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }
        }
        //10
        public async void GetNewsFromCollectionWithId()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Int32 collectionId = DemandInt32("Enter collection id:");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var r = await client.GetAsync(new Uri($"{BaseUrl}/News/Collection/{collectionId}")))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<IEnumerable<NewsItem>>(result);
                        Console.WriteLine("Result:");

                        foreach (var item in objResponse)
                        {
                            Console.WriteLine($"Id:{item.Id}    Title:{item.Title} Link:{item.Link}");
                            Console.WriteLine($"Description:{item.Description} ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }

        }
        //11
        public async void ToCacheState()
        {

        }
        //12
        public async void ToHardState()
        {

        }
        //to Collection where name =...
        //13 +
        public async void AddFeedToCollection()
        {
            try
            {
                Int32 collectionId = DemandInt32("Enter collecion id:");
                Int32 feedId = DemandInt32("Enter feed id:");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    String json = JsonConvert.SerializeObject(feedId, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    Console.WriteLine();
                    await client.PutAsync(BaseUrl + "/Collection/add/" + collectionId, new StringContent(json, Encoding.UTF8, "application/json"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Press Enter!");
            }

        }
        #endregion




        public Controller(Viewer view)
        {
            _view = view;
            _commandMap = new SortedDictionary<UInt32, Action>();
            _commandMap.Add(0, new Action(GetFeedById));
            _commandMap.Add(1, new Action(GetFeeds));
            _commandMap.Add(2, new Action(GetFeedsByCollectionId));
            _commandMap.Add(3, new Action(DeleteFeedById));
            _commandMap.Add(4, new Action(AddFeedWithRerf));
            _commandMap.Add(5, new Action(GetMyCollections));
            _commandMap.Add(6, new Action(GetCollectionById));
            _commandMap.Add(7, new Action(DeleteCollectionById));
            _commandMap.Add(8, new Action(CreateNewCollectionWithName));
            _commandMap.Add(9, new Action(GetNewsByFeedId));
            _commandMap.Add(10, new Action(GetNewsFromCollectionWithId));
            _commandMap.Add(11, new Action(ToCacheState));
            _commandMap.Add(12, new Action(ToHardState));
            _commandMap.Add(13, new Action(AddFeedToCollection));
        }
    }
}
