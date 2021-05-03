using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroSys.Api.Products.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MicroSys.Api.Products.Model;

namespace MicroSys.Api.Products.Providers
{
    public interface IProductProvider
    {
        Task<(bool IsSuccess, IEnumerable<Model.Product> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, Model.Product Product, string ErrorMessage)> GetProductsAsync(int id);

    }
    public class ProductProvider : IProductProvider
    {
        private readonly Db.ProductDbContext _dbContext;
        private readonly ILogger<ProductProvider> _logger;
        private readonly IMapper _mapper;

        public ProductProvider(ProductDbContext dbContext, ILogger<ProductProvider> logger, IMapper mapper )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }
        
        private void SeedData()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                _dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
                _dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 1000 });
                _dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 2000 });
                _dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Get All products from db
        /// </summary>
        /// <returns> a tuple includes:
        /// IsSuccess: if all products are retrived successfully
        /// Products: a collection of all products
        /// ErrorMessage: contains the error message if IsSuccess if false
        /// </returns>
        public async Task<(bool IsSuccess, IEnumerable<Model.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                _logger?.LogInformation("Querying products");
                var products = await _dbContext.Products.ToListAsync();
                if(products != null)
                {
                    var result = _mapper.Map<IEnumerable<Model.Product>>(products);
                    _logger?.LogInformation($"{products.Count} product(s) found");
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }

        }

        /// <summary>
        /// Get a product by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, Model.Product Product, string ErrorMessage)> GetProductsAsync(int id)
        {
            try
            {
                _logger?.LogInformation($"Querying product with id = {id}");
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = _mapper.Map<Model.Product>(product);
                    _logger?.LogInformation($"product found");
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
