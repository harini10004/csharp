using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public interface IInventoryService
    {
        Inventory GetInventoryByProductId(int productId);
        bool UpdateStockQuantity(int productId, int quantityChange);
        bool UpdateStockQuantity(int productId, int quantityChange, SqlConnection connection, SqlTransaction transaction);
        List<Inventory> GetLowStockProducts(int threshold);
        List<Inventory> GetAllInventory();
        Inventory GetInventoryByProductId(int productId, SqlConnection connection = null, SqlTransaction transaction = null);
    }
}
