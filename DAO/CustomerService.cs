using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public class CustomerService:ICustomerService
    {
        public bool AddCustomer(Customer customer)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                try
                {
                    string query = "INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) " +
                                  "VALUES (@FirstName, @LastName, @Email, @Phone, @Address)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)  
                {
                    throw ex;
                }
                
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Customer
                            {
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null
                            };
                        }
                        else
                        {
                            throw new InvalidDataException($"Customer with ID {customerId} not found");
                        }
                    }
                }
            }
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Customers";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null
                            });
                        }
                    }
                }
            }

            return customers;
        }

        public bool UpdateCustomer(Customer customer)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                try
                {
                    string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, " +
                                  "Email = @Email, Phone = @Phone, Address = @Address " +
                                  "WHERE CustomerID = @CustomerID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex) 
                {
                    throw ex;
                }
                
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                try
                {
                    string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidDataException($"Customer with ID {customerId} not found");
                        }
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex) when (ex.Number == 547) // Foreign key constraint violation
                {
                    throw ex;
                }
                
            }
        }

        public List<Customer> GetCustomersByCity(string city)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Customers WHERE Address LIKE @City";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@City", $"%{city}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null
                            });
                        }
                    }
                }
            }

            if (customers.Count == 0)
            {
                throw new InvalidDataException($"No customers found in city: {city}");
            }

            return customers;
        }

        public int CalculateTotalOrders(int customerId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Orders WHERE CustomerID = @CustomerID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);

                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }
    }
}
