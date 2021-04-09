using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Context;
using ProductApi.Models;
using ProductApi.Repository.IRepository;

namespace ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateProduct(Product product)
        {
            context.Product.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            context.Product.Remove(product);
            return Save();
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return context.Product.ToList();
        }

        public Product GetProduct(int id)
        {
            return context.Product.FirstOrDefault(a => a.IdProduct.Equals(id));
        }

        public bool ProductExist(string name)
        {
            return context.Product.Any(a => a.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
        }

        public bool ProductExist(int id)
        {
            return context.Product.Any(a => a.IdProduct.Equals(id));
        }

        public bool Save()
        {
            return context.SaveChanges() >  0? true : false ;
        }

        public bool UpdateProduct(Product product)
        {
            context.Product.Update(product);
            return Save();
        }
    }
}
