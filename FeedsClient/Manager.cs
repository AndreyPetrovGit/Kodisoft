using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FeedsClient.Controllers;
using FeedsClient.Model;
using FeedsClient.View;
namespace FeedsClient
{
    class Manager
    {
        Controller _controller;
        Viewer _view;
        public void Run()
        {
            Console.WriteLine("Enter server uri:");
            String url = "http://localhost:50515/api";//Console.ReadLine();
            Console.WriteLine($"URL={url}");
            _controller.BaseUrl = url;
            bool isAuth = false;
            while (!isAuth)
            {
                try
                {
                    var result = _controller.GetToken();
                    var objResponse = JsonConvert.DeserializeObject<AuthData>(result.Result);
                    _controller.Token = objResponse.access_token;
                    Console.WriteLine($"Добро пожаловать, {objResponse.username}!");
                    isAuth = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не получилось пройти аутентификацию!\n{ex.Data}");
                }
            }

           // _controller.PostFeed("https://www.engadget.com/rss.xml");
            _controller.ManagerLoop();
        }
        public Manager()
        {
            _view = new Viewer();
            _controller = new Controller( _view);
        }
    }
}
