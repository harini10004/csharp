using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    internal class InventoryService:IInventoryService
    {
        public Inventory GetInventoryByProductId(int productId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Inventory WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Inventory
                            {
                                InventoryID = Convert.ToInt32(reader["InventoryID"]),
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                                LastStockUpdate = Convert.ToDateTime(reader["LastStockUpdate"])
                            };
                        }
                        else
                        {
                            throw new InvalidDataException($"Inventory record for product ID {productId} not found");
                        }
                    }
                }
            }
        }
        public Inventory GetInventoryByProductId(int productId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "SELECT * FROM Inventory WHERE ProductID = @ProductID";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@ProductID", productId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Inventory
                        {
                            InventoryID = Convert.ToInt32(reader["InventoryID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                            LastStockUpdate = Convert.ToDateTime(reader["LastStockUpdate"])
                        };
                    }
                    else
                    {
                        throw new InvalidDataException($"Inventory record for product ID {productId} not found");
                    }
                }
            }
        }


        public bool UpdateStockQuantity(int productId, int quantityChange)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                return UpdateStockQuantity(productId, quantityChange, connection, null);
            }
        }

        public bool UpdateStockQuantity(int productId, int quantityChange,
                       SqlConnection connection = null,
                       SqlTransaction transaction = null)
        {
            bool shouldCloseConnection = false;

            try
            {
                if (connection == null)
                {
                    connection = DBUtility.GetConnection();
                    connection.Open();
                    shouldCloseConnection = true;
                }

                string query = @"UPDATE Inventory 
                        SET QuantityInStock = QuantityInStock + @QuantityChange, 
                            LastStockUpdate = GETDATE() 
                        WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.Parameters.AddWithValue("@QuantityChange", quantityChange);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            finally
            {
                if (shouldCloseConnection && connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<Inventory> GetLowStockProducts(int threshold)
        {
            if (threshold < 0)
            {
                throw new InvalidDataException("Threshold cannot be negative");
            }

            List<Inventory> inventoryList = new List<Inventory>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Inventory WHERE QuantityInStock < @Threshold";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Threshold", threshold);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inventoryList.Add(new Inventory
                            {
                                InventoryID = Convert.ToInt32(reader["InventoryID"]),
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                                LastStockUpdate = Convert.ToDateTime(reader["LastStockUpdate"])
                            });
                        }
                    }
                }
            }

            if (inventoryList.Count == 0)
            {
                throw new InvalidDataException($"No products found with stock below {threshold}");
            }

            return inventoryList;
        }

        public List<Inventory> GetAllInventory()
        {
            List<Inventory> inventoryList = new List<Inventory>();

            using (SqlConnection connection = DBUtility.GetConnection())
            {
                string query = "SELECT * FROM Inventory";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inventoryList.Add(new Inventory
                            {
                                InventoryID = Convert.ToInt32(reader["InventoryID"]),
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                                LastStockUpdate = Convert.ToDateTime(reader["LastStockUpdate"])
                            });
                        }
                    }
                }
            }

            return inventoryList;
        }
    }
}
