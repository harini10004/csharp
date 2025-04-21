using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.DAO
{
    internal class DBUtility
    {
        const string connectionString = @"Data Source = HARINI; Initial Catalog = TechShop ; Integrated Security =True ; MultipleActiveResultSets=true;";

        private SqlConnection connectionObject { get; set; }

        //method to open connection
        public static SqlConnection GetConnection()
        {
            SqlConnection connectionObject = new SqlConnection(connectionString);
            try
            {
                connectionObject.Open();
                return connectionObject;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Opening the Connection :{e.Message}");
                return null;
            }
        }
        // method to close the connection

        public static void CloseddbConnection(SqlConnection connectionObject)
        {
            if (connectionObject != null)
            {
                try
                {
                    if (connectionObject.State != ConnectionState.Open)
                    {
                        connectionObject.Close();
                        connectionObject.Dispose();
                        Console.WriteLine("Connection closed");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error closing connection{e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Connection is already null");
            }

        }
    }
}

