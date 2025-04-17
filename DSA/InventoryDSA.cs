using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Model;

namespace TechShop.DSA
{
    internal class InventoryDSA
    {
        private List<Inventory> inventoryItems;
        private SortedList<int, Inventory> inventoryByProductId;

        public InventoryDSA()
        {
            inventoryItems = new List<Inventory>();
            inventoryByProductId = new SortedList<int, Inventory>();
        }

        public void AddInventoryItem(Inventory inventoryItem)
        {
            if (inventoryItems.Any(i => i.InventoryId == inventoryItem.InventoryId))
            {
                throw new TechShopException.DuplicateEntityException($"Inventory item with ID {inventoryItem.InventoryId} already exists.");
            }
            if (inventoryByProductId.ContainsKey(inventoryItem.Product.ProductId))
            {
                throw new TechShopException.DuplicateEntityException($"Inventory for product ID {inventoryItem.Product.ProductId} already exists.");
            }

            inventoryItems.Add(inventoryItem);
            inventoryByProductId.Add(inventoryItem.Product.ProductId, inventoryItem);
        }

        public Inventory GetInventoryById(int inventoryId)
        {
            var inventory = inventoryItems.FirstOrDefault(i => i.InventoryId == inventoryId);
            if (inventory == null)
            {
                throw new TechShopException.EntityNotFoundException($"Inventory item with ID {inventoryId} not found.");
            }
            return inventory;
        }

        public Inventory GetInventoryByProductId(int productId)
        {
            if (inventoryByProductId.TryGetValue(productId, out var inventory))
            {
                return inventory;
            }
            throw new TechShopException.EntityNotFoundException($"Inventory for product ID {productId} not found.");
        }

        public List<Inventory> GetAllInventoryItems()
        {
            return inventoryItems.ToList();
        }

        public void UpdateInventoryItem(Inventory updatedInventory)
        {
            var existingInventory = GetInventoryById(updatedInventory.InventoryId);
            existingInventory.Product = updatedInventory.Product;
            existingInventory.QuantityInStock = updatedInventory.QuantityInStock;
            existingInventory.LastStockUpdate = updatedInventory.LastStockUpdate;

            inventoryByProductId[existingInventory.Product.ProductId] = existingInventory;
        }

        public void DeleteInventoryItem(int inventoryId)
        {
            var inventory = GetInventoryById(inventoryId);
            inventoryItems.Remove(inventory);
            inventoryByProductId.Remove(inventory.Product.ProductId);
        }

        public void AddToInventory(int productId, int quantity)
        {
            var inventory = GetInventoryByProductId(productId);
            inventory.QuantityInStock += quantity;
            inventory.LastStockUpdate = DateTime.Now;
        }

        public void RemoveFromInventory(int productId, int quantity)
        {
            var inventory = GetInventoryByProductId(productId);
            if (inventory.QuantityInStock < quantity)
            {
                throw new TechShopException.InsufficientStockException($"Not enough stock available. Requested: {quantity}, Available: {inventory.QuantityInStock}");
            }
            inventory.QuantityInStock -= quantity;
            inventory.LastStockUpdate = DateTime.Now;
        }

        public bool IsProductAvailable(int productId, int quantity)
        {
            try
            {
                var inventory = GetInventoryByProductId(productId);
                return inventory.QuantityInStock >= quantity;
            }
            catch (TechShopException.EntityNotFoundException)
            {
                return false;
            }
        }

        public List<Product> ListLowStockProducts(int threshold)
        {
            return inventoryItems
                .Where(i => i.QuantityInStock < threshold)
                .Select(i => i.Product)
                .ToList();
        }

        public List<Product> ListOutOfStockProducts()
        {
            return inventoryItems
                .Where(i => i.QuantityInStock == 0)
                .Select(i => i.Product)
                .ToList();
        }

        public decimal GetTotalInventoryValue()
        {
            return inventoryItems.Sum(i => i.GetInventoryValue());
        }
    }
}
