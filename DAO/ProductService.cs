using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public class ProductService:IProductService
    {
        public bool AddProduct(Product product)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "INSERT INTO Products (ProductName, Description, Price) " +
                              "VALUES (@ProductName, @Description, @Price)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Add to inventory with default quantity
                            query = "INSERT INTO Inventory (ProductID, QuantityInStock) " +
                                    "VALUES (SCOPE_IDENTITY(), 0)";
                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.ExecuteNonQuery();
                            return true;
                        }
                        return false;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public Product GetProductById(int productId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Price = Convert.ToDecimal(reader["Price"]),
                                Category = reader["Category"].ToString()
                            };
                        }
                        else
                        {
                            throw new InvalidDataException($"Product with ID {productId} not found");
                        }
                    }
                }
            }
        }
        public Product GetProductById(int productId, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            bool shouldCloseConnection = false;
            SqlConnection localConnection = connection;

            try
            {
                if (localConnection == null)
                {
                    localConnection = DBUtility.GetConnection();
                    shouldCloseConnection = true;
                }

                string query = "SELECT * FROM Products WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, localConnection, transaction))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductID = (int)reader["ProductID"],
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Price = (decimal)reader["Price"],
                                Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : null
                            };
                        }
                        throw new TechshopException("product id not found");
                    }
                }
            }
            finally
            {
                if (shouldCloseConnection && localConnection != null)
                {
                    DBUtility.CloseddbConnection(localConnection);
                }
            }
        }



        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Price = Convert.ToDecimal(reader["Price"]),
                                Category = reader["Category"].ToString()
                            });
                        }
                    }
                }
            }

            return products;
        }

        public bool UpdateProduct(Product product)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "UPDATE Products SET ProductName = @ProductName, " +
                              "Description = @Description, Price = @Price " +
                              "WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidDataException($"Product with ID {product.ProductID} not found");
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

        public bool DeleteProduct(int productId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                try
                {
                    // First check if product has any orders
                    string checkQuery = "SELECT COUNT(*) FROM OrderDetails WHERE ProductID = @ProductID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ProductID", productId);
                        int orderCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (orderCount > 0)
                        {
                            throw new InvalidDataException("Cannot delete product with existing orders");
                        }
                    }

                    // Delete from inventory first due to foreign key constraint
                    string deleteInventoryQuery = "DELETE FROM Inventory WHERE ProductID = @ProductID";
                    using (SqlCommand deleteInventoryCommand = new SqlCommand(deleteInventoryQuery, connection))
                    {
                        deleteInventoryCommand.Parameters.AddWithValue("@ProductID", productId);
                        deleteInventoryCommand.ExecuteNonQuery();
                    }

                    // Then delete the product
                    string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand deleteProductCommand = new SqlCommand(deleteProductQuery, connection))
                    {
                        deleteProductCommand.Parameters.AddWithValue("@ProductID", productId);
                        int rowsAffected = deleteProductCommand.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidDataException($"Product with ID {productId} not found");
                        }
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public List<Product> GetProductsByName(string productName)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE ProductName LIKE @ProductName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", $"%{productName}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }
            }

            if (products.Count == 0)
            {
                throw new InvalidDataException($"No products found with name containing: {productName}");
            }

            return products;
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                throw new InvalidDataException("Minimum price cannot be greater than maximum price");
            }

            List<Product> products = new List<Product>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE Price BETWEEN @MinPrice AND @MaxPrice";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MinPrice", minPrice);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }
            }

            if (products.Count == 0)
            {
                throw new InvalidDataException($"No products found in price range {minPrice:C} to {maxPrice:C}");
            }

            return products;
        }
    }
}
