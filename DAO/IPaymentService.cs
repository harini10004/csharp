using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
  public interface IPaymentService
    {
        bool ProcessPayment(int orderId, decimal amount, string paymentMethod);
        Payment GetPaymentDetails(int paymentId);
        List<Payment> GetPaymentsByOrder(int orderId);
        bool UpdatePaymentStatus(int paymentId, string status);
      
    }
}
