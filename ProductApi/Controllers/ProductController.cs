

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Models.DTO;
using ProductApi.Repository.IRepository;
using System.Collections.Generic;

namespace ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = productRepository.GetAllProduct();
            return Ok(mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public IActionResult GetProductById(int id)
        {
            var product = productRepository.GetProduct(id);
            if (product == null) return NotFound();
            return Ok(mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (productRepository.ProductExist(productDto.Name))
            {
                ModelState.AddModelError("Response", $"el producto con el nombre {productDto.Name} ya existe");
                return StatusCode(404, ModelState);
            }

            var product = mapper.Map<Product>(productDto);
            if (!productRepository.CreateProduct(product))
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
            
            var product = mapper.Map<Product>(productDto);
            if (!productRepository.UpdateProduct(product))
            {
                ModelState.AddModelError("Response", $"Ha ocurrido un error al intentar actualizar el producto {productDto.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();     
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!productRepository.ProductExist(id)) return NotFound();

            var product = productRepository.GetProduct(id);

            if (!productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("Response", $"Ha ocurrido un error al intentar eliminar el producto {product.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
