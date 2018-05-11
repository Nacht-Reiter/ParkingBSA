using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ParkingBSA
{
    public class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instanse { get { return lazy.Value; } }
        private Timer PayTimer = new Timer(3000);
        private Timer LogTimer = new Timer(60000);

        private Parking()
        {
            PayTimer.AutoReset = true;
            PayTimer.Start();
            LogTimer.AutoReset = true;
            LogTimer.Start();
            LogTimer.Elapsed += Log;

        }

        public decimal Balance { get; private set; } = 0;

        public List<Car> CarsList { get; } = new List<Car>();
        public List<Transaction> TransactionsList { get; } = new List<Transaction>();


        public void AddCar(Car car)
        {
            if (car != null && this.FreeSpace() > 0)
            {
                car.Payed += AddIncome;
                car.TransactionMade += AddTransaction;
                PayTimer.Elapsed += car.Pay;
                CarsList.Add(car);
            }
        }

        public void RemoveCar(Car car)
        {
            if (car != null)
            {
                PayTimer.Elapsed -= car.Pay;
                car.Payed -= AddIncome;
                car.TransactionMade -= AddTransaction;
                CarsList.Remove(car);
                
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                TransactionsList.Add(transaction);
            }
            if ((DateTime.Now - TransactionsList[0].TransactionDateTime).Minutes > 0)
            {
                TransactionsList.Remove(TransactionsList[0]);
            }
        }

        public void Log(object sender, ElapsedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("Transactions.log", true))
            {
                foreach(Transaction i in TransactionsList)
                {
                    sw.WriteLine(i.ToString());
                }
            }

        }

        public int FreeSpace()
        {
            return Settings.ParkingSpace - CarsList.Count;
        }

        public void AddIncome(decimal income)
        {
            Balance += income;
        }

    }
}
