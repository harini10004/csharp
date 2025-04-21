using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class Inventory
    {
        private int inventoryID;
        private int productID;
        private int quantityInStock;
        private DateTime lastStockUpdate;

        public int InventoryID
        {
            get { return inventoryID; }
            set { inventoryID = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Product ID must be positive");
                productID = value;
            }
        }

        public int QuantityInStock
        {
            get { return quantityInStock; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative");
                quantityInStock = value;
            }
        }

        public DateTime LastStockUpdate
        {
            get { return lastStockUpdate; }
            set { lastStockUpdate = value; }
        }

        public override string ToString()
        {
            return $"InventoryID: {InventoryID}, ProductID: {ProductID}, Quantity: {QuantityInStock}, LastUpdate: {LastStockUpdate}";
        }
    }
}
