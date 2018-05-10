using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingBSA
{
    class Menu
    {
        static void Main(string[] args)
        {


        }

        private static readonly List<String> MainMenuItems = new List<String>
        {
            "Car Control",
            "Parking Control",
            "Exit"
        };
        private static readonly String MainMenuTopInfo = "Main menu";


        private static readonly List<String> CarControlMenuItems = new List<String>
        {
            "Add car",
            "Remove car",
            "Balance",
            "Back"
        };
        private static readonly String CarControlMenuTopInfo = "Car Control";


        private static readonly List<String> ParkingControlMenuItems = new List<String>
        {
            "Balance",
            "Free Space",
            "Transactions history (1 min)",
            "Transactions log",
            "Back"
        };
        private static readonly String ParkingControlMenuTopInfo = "Parking Control";




        private static int PrintMenu(List<String> menuItems, String topInfo)
        {
            int counter = 0;
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                Console.WriteLine(topInfo + "\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (counter == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(menuItems[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.WriteLine(menuItems[i]);

                }
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    counter--;
                    if (counter == -1)
                    {
                        counter = menuItems.Count - 1;
                    }
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    counter++;
                    if (counter == menuItems.Count)
                    {
                        counter = 0;
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);
            return counter;
        }
    }
}
