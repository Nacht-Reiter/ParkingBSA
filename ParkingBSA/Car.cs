using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ParkingBSA
{
    public enum CarTypes
    {
        Passenger,
        Truck,
        Bus,
        Motorcycle
    }

    public class Car
    {
        public String ID { get; set; }
        public decimal Balance { get; private set; } = 0;


        public CarTypes CarType { get; set; }

        public delegate void CarPayHandler(decimal payment);
        public delegate void TransactionHandler(Transaction transaction);
        public event CarPayHandler Payed;
        public event TransactionHandler TransactionMade;

        public Car(string iD, CarTypes carType, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(iD))
            {
                throw new ArgumentException();
            }

            ID = iD;
            CarType = carType;
            Balance = balance;
        }

        public void RemovePayment(decimal payment)
        {
                Balance -= payment;   
        }

        public void AddIncome(decimal income)
        {
            if (this.IsDebtor())
            {
                Balance += income;
                decimal debt = income;
                if (!IsDebtor())
                {
                    debt -= Balance;
                }
                Payed(debt);
                TransactionMade(new Transaction(DateTime.Now, ID, debt));
            }
            else
            {
                Balance += income;
            }
        }

        public bool IsDebtor()
        {
            if(Balance < 0)
            {
                return true;
            }
            return false;
        }

        public void Pay(object sender, ElapsedEventArgs e)
        {
            decimal cost = Settings.Prices[CarType];
            if (Balance < Settings.Prices[CarType])
            {
                cost *= Settings.Fine;
            }
            else
            {
                Payed(cost);
                TransactionMade(new Transaction(DateTime.Now, ID, cost));
            }
            RemovePayment(cost);
            
        }
        
    }
}
