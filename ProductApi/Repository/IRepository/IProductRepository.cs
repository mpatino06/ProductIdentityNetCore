using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Models;

namespace ProductApi.Repository.IRepository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProduct();
        Product GetProduct(int id);
        bool ProductExist(string name);
        bool ProductExist(int id);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
         
    }
}
