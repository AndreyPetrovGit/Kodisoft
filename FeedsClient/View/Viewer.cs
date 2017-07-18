using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedsClient.Model;
namespace FeedsClient.View
{
    public class Viewer
    {

        public void MenuRenderer()
        {

            Console.WriteLine("_________________________MENU________________________________________________________________________");
            Console.WriteLine("|{0,-20}{1,-30}", "command", "action");
            Console.WriteLine("|----------------------------------------------------------------------------------------------------|");
            Console.WriteLine("|{0,-20}{1,-30}", "0", "Get Feed by id");
            Console.WriteLine("|{0,-20}{1,-30}", "1", "Get all Feeds");
            Console.WriteLine("|{0,-20}{1,-30}", "2", "Get Feed by Collection Id");
            Console.WriteLine("|{0,-20}{1,-30}", "4", "Add Feed");
  //          Console.WriteLine("|{0,-20}{1,-30}", "14", "DeleteFeedById");
            Console.WriteLine("|{0,-20}{1,-30}", "3", "Delete Feed from Collection");
            Console.WriteLine();
            Console.WriteLine("|{0,-20}{1,-30}", "5", "Get Collections");
            Console.WriteLine("|{0,-20}{1,-30}", "8", "Create new Collection");
            Console.WriteLine("|{0,-20}{1,-30}", "13", "Put Feed to Collection");
            Console.WriteLine("|{0,-20}{1,-30}", "7", "Delete Collection by id");
            Console.WriteLine();
            Console.WriteLine("|{0,-20}{1,-30}", "9", "Get News by Feed id");
            Console.WriteLine("|{0,-20}{1,-30}", "10", "Get News by Collection name");
            

            Console.WriteLine("|----------------------------------------------------------------------------------------------------|");
            Console.WriteLine();

        }
        public void ShowNewsItems(IEnumerable<NewsItem> collection)
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "Id", "Title", "Description", "Link");
            Console.WriteLine("-------------------------------------------------------");
            foreach (var item in collection)
            {
                Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", item.Id, item.Title, item.Description, item.Link);
            }
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();

        }
        public void ShowNewsItem(NewsItem item)
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "Id", "Title", "Description", "Link");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", item.Id, item.Title, item.Description, item.Link);
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();

        }
        public void ShowMessage(String message)
        {
            Console.WriteLine(message);
        }
        public void ShowIsSuccessOperation(Boolean isSuccess)
        {
            if (isSuccess)
            {
                Console.WriteLine("Operation completed successfully!");
            }
            else
            {
                Console.WriteLine("Operation was denied!");
            }
        }


        public Viewer()
        {

        }
    }
}
