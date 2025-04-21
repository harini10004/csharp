using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public interface IOrderService
    {
        bool PlaceOrder(Order order, List<OrderDetail> orderDetails);
        Order GetOrderById(int orderId);
        List<Order> GetAllOrders();
        bool UpdateOrder(Order order);
        bool CancelOrder(int orderId);
        List<Order> GetOrdersByCustomer(int customerId);
        List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        decimal CalculateTotalRevenue();
    }
}
