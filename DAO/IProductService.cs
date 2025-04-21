using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public interface IProductService
    {
        bool AddProduct(Product product);
        Product GetProductById(int productId);
        List<Product> GetAllProducts();
        bool UpdateProduct(Product product);
        bool DeleteProduct(int productId);
        List<Product> GetProductsByName(string productName);
        List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Product GetProductById(int productId, SqlConnection connection = null, SqlTransaction transaction = null);
    }
}
