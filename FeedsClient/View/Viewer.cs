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
            Console.WriteLine("|{0,-20}{1,-30}", "0", "Get Feed by id");//+
            Console.WriteLine("|{0,-20}{1,-30}", "1", "Get all Feeds");// +
            Console.WriteLine("|{0,-20}{1,-30}", "2", "Get Feed by collection Id");// +
            Console.WriteLine("|{0,-20}{1,-30}", "3", "Delete Feed from Collection Id"); //
            Console.WriteLine("|{0,-20}{1,-30}", "4", "Add Feed with ref=... "); //@ 
            Console.WriteLine("|{0,-20}{1,-30}", "5", "Get Collections");//by user
            Console.WriteLine("|{0,-20}{1,-30}", "6", "Get Collection by id");
            Console.WriteLine("|{0,-20}{1,-30}", "7", "Delete Collection by id");
            Console.WriteLine("|{0,-20}{1,-30}", "8", "Create new Collection"); //name @
            Console.WriteLine("|{0,-20}{1,-30}", "9", "Get News Where Feed id = ...");
            Console.WriteLine("|{0,-20}{1,-30}", "10", "Get News ForEach Collection with name = ..."); //@
            Console.WriteLine("|{0,-20}{1,-30}", "11", "Change State to Cache //load content form server Cache ");
            Console.WriteLine("|{0,-20}{1,-30}", "12", "Change State to Hard//every query is hard reload");
            Console.WriteLine("|{0,-20}{1,-30}", "13", "Add Feed width id to Collection width id ");//+ 
            Console.WriteLine("|{0,-20}{1,-30}", "14", "DeleteFeedById");
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
