using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class Product
    {
        private int productID;
        private string productName;
        private string description;
        private decimal price;
       public string Category { get; set; }
        public Product() { }
        public Product(string productName, string description, decimal price, string category)
        {
            ProductName = productName;
            Description = description;
            Price = price;
            Category = category;
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty");
                productName = value;
            }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than 0");
                price = value;
            }
        }

        public override string ToString()
        {
            return $"ProductID: {ProductID}, Name: {ProductName}, Description: {Description}, Price: {Price:C},Category:{Category}";
        }
    }
}
