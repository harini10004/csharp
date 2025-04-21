using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class Payment
    {
        private int paymentID;
        private int orderID;
        private DateTime paymentDate;
        private decimal amount;
        private string paymentMethod;
        private string status;

        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        public int OrderID
        {
            get { return orderID; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Order ID must be positive");
                orderID = value;
            }
        }

        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Amount must be positive");
                amount = value;
            }
        }

        public string PaymentMethod
        {
            get { return paymentMethod; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Payment method cannot be empty");
                paymentMethod = value;
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
            return $"PaymentID: {PaymentID}, OrderID: {OrderID}, Date: {PaymentDate:d}, Amount: {Amount:C}, Method: {PaymentMethod}, Status: {Status}";
        }
    }
}
