using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Model;

namespace TechShop.DSA
{
    internal class OrderDetailDSA
    {
        private List<OrderDetail> orderDetails;

        public OrderDetailDSA()
        {
            orderDetails = new List<OrderDetail>();
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetails.Any(od => od.OrderDetailId == orderDetail.OrderDetailId))
            {
                throw new TechShopException.DuplicateEntityException($"Order detail with ID {orderDetail.OrderDetailId} already exists.");
            }
            orderDetails.Add(orderDetail);
        }

        public OrderDetail GetOrderDetailById(int orderDetailId)
        {
            var orderDetail = orderDetails.FirstOrDefault(od => od.OrderDetailId == orderDetailId);
            if (orderDetail == null)
            {
                throw new TechShopException.EntityNotFoundException($"Order detail with ID {orderDetailId} not found.");
            }
            return orderDetail;
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return orderDetails.ToList();
        }

        public List<OrderDetail> GetOrderDetailsByOrder(int orderId)
        {
            return orderDetails.Where(od => od.Order.OrderId == orderId).ToList();
        }

        public void UpdateOrderDetail(OrderDetail updatedOrderDetail)
        {
            var existingOrderDetail = GetOrderDetailById(updatedOrderDetail.OrderDetailId);
            existingOrderDetail.Product = updatedOrderDetail.Product;
            existingOrderDetail.Quantity = updatedOrderDetail.Quantity;
            existingOrderDetail.Discount = updatedOrderDetail.Discount;
        }

        public void DeleteOrderDetail(int orderDetailId)
        {
            var orderDetail = GetOrderDetailById(orderDetailId);
            orderDetails.Remove(orderDetail);
        }
    }
}
