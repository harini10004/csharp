using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Model
{
    internal class Inventory
    {
         int inventoryId;
         Product product;
        int quantityInStock;
        DateTime lastStockUpdate;

        public Inventory(int inventoryId, Product product, int quantityInStock, DateTime lastStockUpdate)
        {
            InventoryId = inventoryId;
            Product = product;
            QuantityInStock = quantityInStock;
            LastStockUpdate = lastStockUpdate;
        }

        public int InventoryId
        {
            get { return inventoryId; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Inventory ID must be positive.");
                inventoryId = value;
            }
        }

        public Product Product
        {
            get { return product; }
            set
            {
                if (value == null)
                    throw new TechShopException.InvalidDataException("Product cannot be null.");
                product = value;
            }
        }

        public int QuantityInStock
        {
            get { return quantityInStock; }
            set
            {
                if (value < 0)
                    throw new TechShopException.InvalidDataException("Quantity cannot be negative.");
                quantityInStock = value;
            }
        }

        public DateTime LastStockUpdate
        {
            get { return lastStockUpdate; }
            set
            {
                if (value > DateTime.Now)
                    throw new TechShopException.InvalidDataException("Last stock update cannot be in the future.");
                lastStockUpdate = value;
            }
        }

        public decimal GetInventoryValue()
        {
            return Product.Price * QuantityInStock;
        }

        public override string ToString()
        {
            return $"Inventory ID: {InventoryId}\t Product: {Product.ProductName}\t Quantity: {QuantityInStock}\t Last Updated: {LastStockUpdate:d}\t Value: {GetInventoryValue():C}";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Inventory otherInventory)
            {
                return this.InventoryId == otherInventory.InventoryId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return InventoryId.GetHashCode();
        }
    }
}
