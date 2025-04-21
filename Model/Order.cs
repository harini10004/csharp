using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class Order
    {
        private int orderID;
        private int customerID;
        private DateTime orderDate;
        private decimal totalAmount;
        private string status;

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public int CustomerID
        {
            get { return customerID; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Customer ID must be positive");
                customerID = value;
            }
        }

        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total amount cannot be negative");
                totalAmount = value;
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be empty");
                status = value;
            }
        }

        public override string ToString()
        {
            return $"OrderID: {OrderID}, CustomerID: {CustomerID}, Date: {OrderDate:d}, Total: {TotalAmount:C}, Status: {Status}";
        }
    }
}
