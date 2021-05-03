using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MicroSys.Api.Products.Providers;

namespace MicroSys.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductProvider _productProvider;

        public ProductController(IProductProvider productProvider)
        {
            _productProvider = productProvider;            
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productProvider.GetProductsAsync();
            if(products.IsSuccess)
            {
                return Ok(products.Products);
            }
            return NotFound();            
        }

        //[HttpGet("{id}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productProvider.GetProductsAsync(id);
            if (product.IsSuccess)
            {
                return Ok(product.Product);
            }
            return NotFound();
        }
    }
}