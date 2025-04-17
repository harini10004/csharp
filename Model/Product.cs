using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Model
{
    internal class Product
    {
        private int productId;
        private string productName;
        private string category;
        private decimal price;
        private string description;

        public Product(int productId, string productName, string category, decimal price, string description)
        {
            ProductId = productId;
            ProductName = productName;
            Category = category;
            Price = price;
            Description = description;
        }

        public int ProductId
        {
            get { return productId; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Product ID must be greater than zero.");
                productId = value;
            }
        }

        public string ProductName
        {
            get { return productName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Product name cannot be empty.");
                if (value.Any(char.IsDigit))
                    throw new TechShopException.InvalidDataException("Product name cannot contain numbers.");

                productName = value;
            }
        }

        public string Category
        {
            get { return category; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Category cannot be empty.");
                category = value;
            }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new TechShopException.InvalidDataException("Price cannot be negative.");

                price = value;
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Description cannot be empty.");
                description = value;
            }
        }

        public void UpdateProductInfo(string category, decimal price, string description)
        {
            Category = category;
            Price = price;
            Description = description;
        }

        public override string ToString()
        {
            return $"Product ID: {ProductId}\t Name: {ProductName}\t Category: {Category}\t Price: {Price:F2}\t Description: {Description}";
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Product otherProduct)
            {
                return this.ProductId == otherProduct.ProductId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }
}
