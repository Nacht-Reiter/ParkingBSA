using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingBSA
{
    class Menu
    {
        static void Main(string[] args)
        {
            MainMenu();
        }
        private delegate void MethodHandler();
        private static Parking Parking { get; set; } = Parking.Instanse;

        private static readonly List<String> MainMenuItems = new List<String>
        {
            "Car Control",
            "Parking Control",
            "Exit"
        };
        private static readonly String MainMenuTopInfo = "Main menu";
        private static readonly List<MethodHandler> MainMenuMethods = new List<MethodHandler>
        {
            CarControlMenu,
            ParkingControlMenu,
            Exit
        };


        private static readonly List<String> CarControlMenuItems = new List<String>
        {
            "Add car",
            "Remove car",
            "Car Balance",
            "Back"
        };
        private static readonly String CarControlMenuTopInfo = "Car Control";
        private static readonly List<MethodHandler> CarControlMenuMethods = new List<MethodHandler>
        {
            AddCar,
            RemoveCar,
            CarBalance,
            Back
        };


        private static readonly List<String> ParkingControlMenuItems = new List<String>
        {
            "Balance",
            "Free Space",
            "Transactions history (1 min)",
            "Transactions log",
            "Back"
        };
        private static readonly String ParkingControlMenuTopInfo = "Parking Control";
        private static readonly List<MethodHandler> ParkingControlMenuItemsMethods = new List<MethodHandler>
        {
            Balance,
            FreeSpace,
            History,
            Log,
            Back
        };

        private static void MainMenu()
        {
            MainMenuMethods[PrintMenu(MainMenuItems, MainMenuTopInfo)]();
        }

        private static void CarControlMenu()
        {
            CarControlMenuMethods[PrintMenu(CarControlMenuItems, CarControlMenuTopInfo)]();
        }

        private static void ParkingControlMenu()
        {
            ParkingControlMenuItemsMethods[PrintMenu(ParkingControlMenuItems, ParkingControlMenuTopInfo)]();
        }

        private static void Exit()
        {

        }

        private static void AddCar()
        {

            CarTypes carType = CarTypeChoseMenu();
            Console.Clear();
            Console.WriteLine($"Car type: {carType}");

            Console.Write("Enter Car`s ID: ");
            String ID = Console.ReadLine();
            Console.Write("Enter start balance: ");
            decimal balance = decimal.Parse(Console.ReadLine());
            
            Parking.AddCar(new Car(ID, carType, balance));
            Console.WriteLine("Car Added");
            Console.ReadKey();
            MainMenu();
        }

        private static void RemoveCar()
        {
            Car car = CarChoseMenu();
            Parking.RemoveCar(car);
            Console.Clear();
            Console.WriteLine("Car removed");
            Console.ReadKey();
            MainMenu();
        }

        private static void CarBalance()
        {
            Car car = CarChoseMenu();
            Console.Clear();
            Console.WriteLine($"Balanse: {car.Balance}");
            Console.ReadKey();
            MainMenu();
        }

        private static void Back()
        {
            MainMenu();
        }

        private static void Balance()
        {
            Console.Clear();
            Console.WriteLine($"Balanse: {Parking.Balance}");
            Console.ReadKey();
            MainMenu();
        }

        private static void FreeSpace()
        {
            Console.Clear();
            Console.WriteLine($"Free space: {Parking.FreeSpace()} cars");
            Console.ReadKey();
            MainMenu();
        }

        private static void History()
        {
            Console.Clear();
            Console.WriteLine($"{"Car",-12} {"Sum",-5} {"Date",-10} {"Time"}");
            foreach(Transaction i in Parking.TransactionsList)
            {
                Console.WriteLine(i.ToString());
            }
            Console.ReadKey();
            MainMenu();
        }

        private static void Log()
        {
            Console.Clear();
            Console.WriteLine($"{"Car",-12} {"Sum",-5} {"Date",-10} {"Time"}");
            using (StreamReader sr = new StreamReader("Transactions.log"))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
            Console.ReadKey();
            MainMenu();
        }

        private static Car CarChoseMenu()
        {
            List<String> items = new List<string>();
            foreach(Car i in Parking.CarsList)
            {
                items.Add(i.ID);
            }
            int result = PrintMenu(items, "Choose car");
            return Parking.CarsList[result];
        }

        private static CarTypes CarTypeChoseMenu()
        {
            List<String> items = new List<string>();
            foreach (CarTypes i in Enum.GetValues(typeof(CarTypes)))
            {
                items.Add(i.ToString(""));
            }
            int result = PrintMenu(items, "Choose type of car");
            return (CarTypes)Enum.GetValues(typeof(CarTypes)).GetValue(result);
        }



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
