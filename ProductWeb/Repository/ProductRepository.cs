using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ProductWEB.Models;
using ProductWEB.Repository.IRepository;

namespace ProductWEB.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }
    }
}
