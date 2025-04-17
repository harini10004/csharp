using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Model;

namespace TechShop.DSA
{
    internal class OrderDSA
    {
        private List<Order> orders;

        public OrderDSA()
        {
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (orders.Any(o => o.OrderId == order.OrderId))
            {
                throw new TechShopException.DuplicateEntityException($"Order with ID {order.OrderId} already exists.");
            }
            orders.Add(order);
        }

        public Order GetOrderById(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new TechShopException.EntityNotFoundException($"Order with ID {orderId} not found.");
            }
            return order;
        }

        public List<Order> GetAllOrders()
        {
            return orders.ToList();
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return orders.Where(o => o.Customer.CustomerId == customerId).ToList();
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = GetOrderById(orderId);
            order.Status = newStatus;
        }

        public void CancelOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            orders.Remove(order);
        }

        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                         .OrderBy(o => o.OrderDate)
                         .ToList();
        }

        public decimal CalculateTotalAmount(int orderId, OrderDetailDSA orderDetailDSA)
        {
            var orderDetails = orderDetailDSA.GetOrderDetailsByOrder(orderId);
            return orderDetails.Sum(od => od.CalculateSubtotal());
        }
    }
}
