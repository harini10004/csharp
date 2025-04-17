using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Model;

namespace TechShop.DSA
{
    internal class ProductDSA
    {
        private List<Product> products;

        public ProductDSA()
        {
            products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            if (products.Any(p => p.ProductId == product.ProductId))
            {
                throw new TechShopException.DuplicateEntityException($"Product with ID {product.ProductId} already exists.");
            }
            products.Add(product);
        }

        public Product GetProductById(int productId)
        {
            var product = products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new TechShopException.EntityNotFoundException($"Product with ID {productId} not found.");
            }
            return product;
        }

        public List<Product> GetAllProducts()
        {
            return products.ToList();
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = GetProductById(updatedProduct.ProductId);
            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
        }

        public void DeleteProduct(int productId)
        {
            var product = GetProductById(productId);
            products.Remove(product);
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            return products.Where(p =>
                p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
