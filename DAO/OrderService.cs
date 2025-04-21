using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    internal class OrderService:IOrderService
    {

        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;


        public OrderService(IProductService productService, IInventoryService inventoryService)
        {
            _productService = productService;
            _inventoryService = inventoryService;
        }

     public OrderService() { }

        public bool PlaceOrder(Order order, List<OrderDetail> orderDetails)
        {
            if (orderDetails == null || orderDetails.Count == 0)
            {
                throw new IncompleteOrderException("Order must contain at least one product");
            }

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                // Get new connection
                connection = DBUtility.GetConnection();
                if (connection == null)
                {
                    throw new TechshopException("Could not establish database connection");
                }

                transaction = connection.BeginTransaction();

                // Insert the order
                string orderQuery = @"INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, Status) 
                                OUTPUT INSERTED.OrderID
                                VALUES (@CustomerID, @OrderDate, @TotalAmount, @Status)";

                int orderId;
                using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection, transaction))
                {
                    orderCommand.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    orderCommand.Parameters.AddWithValue("@Status", order.Status);

                    orderId = (int)orderCommand.ExecuteScalar();
                }

                // Process each order detail
                foreach (var detail in orderDetails)
                {
                    // Verify product exists - using shared connection
                    var product = _productService.GetProductById(detail.ProductID, connection, transaction);
                    detail.UnitPrice = product.Price;

                    // Check inventory - using shared connection
                    var inventory = _inventoryService.GetInventoryByProductId(detail.ProductID, connection, transaction);
                    if (inventory.QuantityInStock < detail.Quantity)
                    {
                        throw new InsufficientStockException(
                            $"Insufficient stock for product {product.ProductName}. " +
                            $"Available: {inventory.QuantityInStock}, Requested: {detail.Quantity}");
                    }

                    // Insert order detail
                    string detailQuery = @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) 
                                     VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";

                    using (SqlCommand detailCommand = new SqlCommand(detailQuery, connection, transaction))
                    {
                        detailCommand.Parameters.AddWithValue("@OrderID", orderId);
                        detailCommand.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        detailCommand.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);

                        detailCommand.ExecuteNonQuery();
                    }

                    // Update inventory - using shared connection
                    _inventoryService.UpdateStockQuantity(detail.ProductID, -detail.Quantity, connection, transaction);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new TechshopException($"Error placing order: {ex.Message}");
            }
            finally
            {
                // Ensure connection is closed
                if (connection != null)
                {
                    DBUtility.CloseddbConnection(connection);
                }
            }
        }

        public Order GetOrderDetails(int orderId)
        {
            try
            {
                SqlConnection connection = DBUtility.GetConnection();
                string query = "SELECT * FROM Orders WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Order
                            {
                                OrderID = (int)reader["OrderID"],
                                CustomerID = (int)reader["CustomerID"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = reader["Status"].ToString()
                            };
                        }
                        else
                        {
                            throw new OrderNotFoundException($"Order with ID {orderId} not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error retrieving order: {ex.Message}");
            }
        }

        public Order GetOrderById(int orderId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Order
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Status = reader["Status"].ToString()
                            };
                        }
                        else
                        {
                            throw new InvalidDataException($"Order with ID {orderId} not found");
                        }
                    }
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Orders";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public bool UpdateOrder(Order order)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "UPDATE Orders SET CustomerID = @CustomerID, OrderDate = @OrderDate, " +
                              "TotalAmount = @TotalAmount, Status = @Status " +
                              "WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);
                    command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    command.Parameters.AddWithValue("@Status", order.Status);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidDataException($"Order with ID {order.OrderID} not found");
                        }
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        public bool UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                SqlConnection connection = DBUtility.GetConnection();
                string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    command.Parameters.AddWithValue("@Status", status);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new OrderNotFoundException($"Order with ID {orderId} not found");
                    }
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new TechshopException($"Error updating order status: {ex.Message}");
            }
        }

        public bool CancelOrder(int orderId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // 1. Get order details to restore inventory
                    string getDetailsQuery = "SELECT ProductID, Quantity FROM OrderDetails WHERE OrderID = @OrderID";
                    List<OrderDetail> details = new List<OrderDetail>();

                    using (SqlCommand getDetailsCommand = new SqlCommand(getDetailsQuery, connection, transaction))
                    {
                        getDetailsCommand.Parameters.AddWithValue("@OrderID", orderId);

                        using (SqlDataReader reader = getDetailsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                details.Add(new OrderDetail
                                {
                                    ProductID = Convert.ToInt32(reader["ProductID"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"])
                                });
                            }
                        }
                    }

                    if (details.Count == 0)
                    {
                        throw new InvalidDataException($"No order details found for order ID {orderId}");
                    }

                    // 2. Restore inventory for each product
                    foreach (var detail in details)
                    {
                        _inventoryService.UpdateStockQuantity(detail.ProductID, detail.Quantity, connection, transaction);
                    }

                    // 3. Update order status to 'Cancelled'
                    string updateOrderQuery = "UPDATE Orders SET Status = 'Cancelled' WHERE OrderID = @OrderID";
                    using (SqlCommand updateOrderCommand = new SqlCommand(updateOrderQuery, connection, transaction))
                    {
                        updateOrderCommand.Parameters.AddWithValue("@OrderID", orderId);
                        int rowsAffected = updateOrderCommand.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidDataException($"Order with ID {orderId} not found");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                   throw ex; 
                }
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Orders WHERE CustomerID = @CustomerID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }

            if (orders.Count == 0)
            {
                throw new InvalidDataException($"No orders found for customer ID {customerId}");
            }

            return orders;
        }

        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidDataException("Start date cannot be after end date");
            }

            List<Order> orders = new List<Order>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Orders WHERE OrderDate BETWEEN @StartDate AND @EndDate";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }

            if (orders.Count == 0)
            {
                throw new InvalidDataException($"No orders found between {startDate:d} and {endDate:d}");
            }

            return orders;
        }

        public decimal CalculateTotalRevenue()
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT SUM(TotalAmount) FROM Orders WHERE Status = 'Completed'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }
    }
}
