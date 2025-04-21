using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class OrderDetail
    {
        private int orderDetailID;
        private int orderID;
        private int productID;
        private int quantity;
        private decimal unitPrice;

        public int OrderDetailID
        {
            get { return orderDetailID; }
            set { orderDetailID = value; }
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

        public int ProductID
        {
            get { return productID; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Product ID must be positive");
                productID = value;
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be positive");
                quantity = value;
            }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Unit price must be positive");
                unitPrice = value;
            }
        }

        public override string ToString()
        {
            return $"OrderDetailID: {OrderDetailID}, OrderID: {OrderID}, ProductID: {ProductID}, Quantity: {Quantity}, UnitPrice: {UnitPrice:C}";
        }
    }
}
