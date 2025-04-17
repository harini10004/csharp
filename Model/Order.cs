using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Model
{
    internal class Order
    {
        private int orderId;
        private Customer customer;
        private DateTime orderDate;
        private decimal totalAmount;
        private string status;

        public Order(int orderId, Customer customer, DateTime orderDate, decimal totalAmount, string status)
        {
            OrderId = orderId;
            Customer = customer;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            Status = status;
        }

        public int OrderId
        {
            get { return orderId; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Order ID must be positive.");
                orderId = value;
            }
        }

        public Customer Customer
        {
            get { return customer; }
            set
            {
                if (value == null)
                    throw new TechShopException.InvalidDataException("Customer cannot be null.");
                customer = value;
            }
        }

        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new TechShopException.InvalidDataException("Order date cannot be in the future.");
                orderDate = value;
            }
        }

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                if (value < 0)
                    throw new TechShopException.InvalidDataException("Total amount cannot be negative.");
                totalAmount = value;
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Status cannot be empty.");
                status = value;
            }
        }

        public override string ToString()
        {
            return $"Order ID: {OrderId}\t Customer: {Customer.FirstName} {Customer.LastName}\t Date: {OrderDate:d}\t Total: ₹{TotalAmount:F2}\t Status: {Status}";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Order otherOrder)
            {
                return this.OrderId == otherOrder.OrderId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode();
        }
    }
}
