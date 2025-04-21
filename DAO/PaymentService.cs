using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;


namespace Tech.DAO
{
    internal class PaymentService:IPaymentService
    {
        public bool ProcessPayment(int orderId, decimal amount, string paymentMethod)
        {
            try
            {
                // First verify the order exists and get its total amount
                var orderService = new OrderService();
                var order = orderService.GetOrderById(orderId);

                if (order.Status == "Cancelled")
                {
                    throw new PaymentFailedException("Cannot process payment for cancelled order");
                }

                if (amount != order.TotalAmount)
                {
                    throw new PaymentFailedException($"Payment amount {amount} does not match order total {order.TotalAmount}");
                }

                // Simulate payment processing (in a real system, this would call a payment gateway)
                bool paymentSuccess = SimulatePaymentProcessing(paymentMethod);

                if (!paymentSuccess)
                {
                    throw new PaymentFailedException("Payment was declined by the payment processor");
                }
                SqlConnection connection = DBUtility.GetConnection();
                // Record the payment
                string query = "INSERT INTO Payments (OrderID, PaymentDate, Amount, PaymentMethod, Status) " +
                              "VALUES (@OrderID, GETDATE(), @Amount, @PaymentMethod, 'Completed')";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new PaymentFailedException("Failed to record payment");
                    }
                }

                // Update order status to Paid
                orderService.UpdateOrderStatus(orderId, "Paid");

                return true;
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error processing payment: {ex.Message}");
            }
        }

        public Payment GetPaymentDetails(int paymentId)
        {
            try
            {
                SqlConnection connection = DBUtility.GetConnection();
                string query = "SELECT * FROM Payments WHERE PaymentID = @PaymentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", paymentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Payment
                            {
                                PaymentID = (int)reader["PaymentID"],
                                OrderID = (int)reader["OrderID"],
                                PaymentDate = (DateTime)reader["PaymentDate"],
                                Amount = (decimal)reader["Amount"],
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                        }
                        else
                        {
                            throw new TechshopException($"Payment with ID {paymentId} not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error retrieving payment: {ex.Message}");
            }
        }

        public List<Payment> GetPaymentsByOrder(int orderId)
        {
            try
            {
                SqlConnection connection = DBUtility.GetConnection();
                List<Payment> payments = new List<Payment>();
                string query = "SELECT * FROM Payments WHERE OrderID = @OrderID ORDER BY PaymentDate DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                PaymentID = (int)reader["PaymentID"],
                                OrderID = (int)reader["OrderID"],
                                PaymentDate = (DateTime)reader["PaymentDate"],
                                Amount = (decimal)reader["Amount"],
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
                return payments;
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error retrieving payments: {ex.Message}");
            }
        }

        public bool UpdatePaymentStatus(int paymentId, string status)
        {
            try
            {
                SqlConnection connection = DBUtility.GetConnection();
                string query = "UPDATE Payments SET Status = @Status WHERE PaymentID = @PaymentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", paymentId);
                    command.Parameters.AddWithValue("@Status", status);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new TechshopException($"Payment with ID {paymentId} not found");
                    }
                    return rowsAffected > 0;
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error updating payment status: {ex.Message}");
            }
        }

        private bool SimulatePaymentProcessing(string paymentMethod)
        {
            // Simulate random payment failures for demonstration
            Random random = new Random();
            return random.Next(1, 10) > 2; // 80% success rate
        }
    }
}
