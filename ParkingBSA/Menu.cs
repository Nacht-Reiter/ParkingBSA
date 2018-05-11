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

                                                                               //Main Menu atributes
        private static readonly List<String> MainMenuItems = new List<String>  //Shown into console to choose
        {
            "Car Control",
            "Parking Control",
            "Exit"
        };
        private static readonly String MainMenuTopInfo = "Main menu";          //Title
        private static readonly List<MethodHandler> MainMenuMethods = new List<MethodHandler>
        {
            CarControlMenu,                                                    //Method, which works when choosen
            ParkingControlMenu,
            Exit
        };                                                                     //*************


                                                                               //Car Control Menu atributes
        private static readonly List<String> CarControlMenuItems = new List<String>
        {                                                                      //Look Main Menu atributes for details
            "Add car",
            "Remove car",
            "Refill the balance",
            "Car Balance",
            "Back"
        };
        private static readonly String CarControlMenuTopInfo = "Car Control";
        private static readonly List<MethodHandler> CarControlMenuMethods = new List<MethodHandler>
        {
            AddCar,
            RemoveCar,
            RefillBalance,
            CarBalance,
            Back
        };                                                                     //*************

        //Car Control Menu atributes
        private static readonly List<String> ParkingControlMenuItems = new List<String>
        {                                                                      //Look Main Menu atributes for details
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
        };                                                                     //*************

        private static void MainMenu() //Shows Main Menu
        {
            MainMenuMethods[PrintMenu(MainMenuItems, MainMenuTopInfo)]();
        }

        private static void CarControlMenu() //Shows  Car ControlMenu
        {
            CarControlMenuMethods[PrintMenu(CarControlMenuItems, CarControlMenuTopInfo)]();
        }

        private static void ParkingControlMenu() //Shows  Parking Control Menu
        {
            ParkingControlMenuItemsMethods[PrintMenu(ParkingControlMenuItems, ParkingControlMenuTopInfo)]();
        }

        private static void Exit() // Exit app
        {

        }

        private static void AddCar() // Adding car to parking
        {

            CarTypes carType = CarTypeChoseMenu();
            Console.Clear();
            Console.WriteLine($"Car type: {carType}");

            try
            {
                Console.Write("Enter Car`s ID: ");
                String ID = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ID))
                {
                    throw new ArgumentException("Invalid ID");
                }

                Console.Write("Enter start balance: ");
                decimal balance = decimal.Parse(Console.ReadLine());
                if (balance < 0)
                {
                    throw new ArgumentException("Invalid balance, enter positive or zero balance");
                }

                Parking.AddCar(new Car(ID, carType, balance));
                Console.WriteLine("Car Added");
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.WriteLine("Invalid balance format");
            }
            catch (ArgumentNullException ex)
            {
                Console.Clear();
                Console.WriteLine("Balance not entered");
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
            
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Invalid data");
            }
            Console.ReadKey();
            MainMenu();
        }

        private static void RemoveCar() // Removing car to from parking
        {
            try
            {
                Car car = CarChoseMenu();
                Parking.RemoveCar(car);
                Console.Clear();
                Console.WriteLine("Car removed");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("There is no car");
            }
            catch (ArithmeticException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            MainMenu();
        }

        private static void RefillBalance() //Refill car`s balance
        {
            try
            {
                Console.Clear();
                Console.Write("Enter the sum: ");
                decimal sum = decimal.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.WriteLine("Invalid sum format");
            }
            catch (ArgumentNullException ex)
            {
                Console.Clear();
                Console.WriteLine("Sum not entered");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Invalid sum");
            }
            MainMenu();
        }


        private static void CarBalance() // Show car`s balance
        {
            try
            {
                Car car = CarChoseMenu();
                Console.Clear();
                Console.WriteLine($"Balanse: {car.Balance}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.Clear();
                Console.WriteLine("There is no car");
            }
            Console.ReadKey();
            MainMenu();
        }

        private static void Back() // Return to Main Menu
        {
            MainMenu();
        }

        private static void Balance() // Show parking balance
        {
            Console.Clear();
            Console.WriteLine($"Balance: {Parking.Balance}");
            Console.ReadKey();
            MainMenu(); 
        }

        private static void FreeSpace() // Show parking free space
        {
            Console.Clear();
            Console.WriteLine($"Free space: {Parking.FreeSpace()} cars");
            Console.ReadKey();
            MainMenu();
        }

        private static void History() // Show Transactions history (1 min)
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

        private static void Log() // Show Transaction.log
        {
            Console.Clear();
            try
            {
                using (StreamReader sr = new StreamReader("Transactions.log"))
                {
                    Console.WriteLine($"{"Car",-12} {"Sum",-5} {"Date",-10} {"Time"}");
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Transactions.log is empty");
            }
            Console.ReadKey();
            MainMenu();
        }

        private static Car CarChoseMenu() //Menu, that shown before removing or editing car to choose right car 
        {
            List<String> items = new List<string>();
            
            foreach(Car i in Parking.CarsList)
            {
                items.Add(i.ID);
            }
            if (items.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            int result = PrintMenu(items, "Choose car");
            return Parking.CarsList[result];
        }

        private static CarTypes CarTypeChoseMenu() //Menu, that shown during adding car to choose right car type 
        {
            List<String> items = new List<string>();
            foreach (CarTypes i in Enum.GetValues(typeof(CarTypes)))
            {
                items.Add(i.ToString(""));
            }
            int result = PrintMenu(items, "Choose type of car");
            return (CarTypes)Enum.GetValues(typeof(CarTypes)).GetValue(result);
        }


        private static int PrintMenu(List<String> menuItems, String topInfo) //Method, that allows to select menu strings
                                                                             //First argument - list of menu items to select
        {                                                                    //Second argument - title
            int counter = 0;                                                 //returns index of decision
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
