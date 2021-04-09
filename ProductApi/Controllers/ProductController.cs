using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Models.DTO;
using ProductApi.Repository.IRepository;

namespace ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IProductRepository ProductRepository { get; }
        public IMapper Mapper { get; }

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            ProductRepository = productRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = ProductRepository.GetAllProduct();
            return Ok(Mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public IActionResult GetProductById(int id)
        {
            var product = ProductRepository.GetProduct(id);
            if (product == null) return NotFound();
            return Ok(Mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (ProductRepository.ProductExist(productDto.Name))
            {
                ModelState.AddModelError("Response", $"el producto con el nombre {productDto.Name} ya existe");
                return StatusCode(404, ModelState);
            }

            var product = Mapper.Map<Product>(productDto);
            if (!ProductRepository.CreateProduct(product))
            {
                ModelState.AddModelError("Response", $"Ha ocurrido un error al intentar guardar el producto {productDto.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetProductById", new { id = product.IdProduct }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id,[FromBody] ProductDto productDto)
        {
            if (id != productDto.IdProduct) return BadRequest(ModelState);
            
            var product = Mapper.Map<Product>(productDto);
            if (!ProductRepository.UpdateProduct(product))
            {
                ModelState.AddModelError("Response", $"Ha ocurrido un error al intentar actualizar el producto {productDto.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();     
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!ProductRepository.ProductExist(id)) return NotFound();

            var product = ProductRepository.GetProduct(id);

            if (!ProductRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("Response", $"Ha ocurrido un error al intentar eliminar el producto {product.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
