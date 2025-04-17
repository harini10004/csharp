using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Model
{
    internal class OrderDetail
    {
        private int orderDetailId;
        private Order order;
        private Product product;
        private int quantity;
        private decimal discount;

        public OrderDetail(int orderDetailId, Order order, Product product, int quantity, decimal discount)
        {
            OrderDetailId = orderDetailId;
            Order = order;
            Product = product;
            Quantity = quantity;
            Discount = discount;
        }

        public int OrderDetailId
        {
            get { return orderDetailId; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Order detail ID must be positive.");
                orderDetailId = value;
            }
        }

        public Order Order
        {
            get { return order; }
            set
            {
                if (value == null)
                    throw new TechShopException.InvalidDataException("Order cannot be null.");
                order = value;
            }
        }

        public Product Product
        {
            get { return product; }
            set
            {
                if (value == null)
                    throw new TechShopException.InvalidDataException("Product cannot be null.");
                product = value;
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Quantity must be positive.");
                quantity = value;
            }
        }

        public decimal Discount
        {
            get { return discount; }
            set
            {
                if (value < 0 || value > 1)
                    throw new TechShopException.InvalidDataException("Discount must be between 0 and 1.");
                discount = value;
            }
        }

        public decimal CalculateSubtotal()
        {
            return Product.Price * Quantity * (1 - Discount);
        }

        public override string ToString()
        {
            return $"Order Detail ID: {OrderDetailId}\t Product: {Product.ProductName}\t Quantity: {Quantity}\t Discount: {Discount:P}\t Subtotal: ₹{CalculateSubtotal():F2}";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is OrderDetail otherDetail)
            {
                return this.OrderDetailId == otherDetail.OrderDetailId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return OrderDetailId.GetHashCode();
        }
    }
}
