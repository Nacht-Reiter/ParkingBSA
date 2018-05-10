using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public event CarPayHandler Payed;

        public Car(string iD, CarTypes carType)
        {
            ID = iD;
            CarType = carType;
            Payed += RemovePayment;
        }

        public void RemovePayment(decimal payment)
        {
                Balance -= payment;   
        }

        public void AddIncome(decimal income)
        {
            Balance += income;
        }

        public bool IsDebtor()
        {
            if(Balance < 0)
            {
                return true;
            }
            return false;
        }

        public void Pay()
        {
            decimal cost = Settings.Prices[CarType];
            if (Balance < Settings.Prices[CarType])
            {
                cost *= Settings.Fine;
            }
            Payed(cost);
        }
    }
}
