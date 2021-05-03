using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MicroSys.Api.Customers.Db;
using MicroSys.Api.Customers.Model;

namespace MicroSys.Api.Customers.Providers
{
    public interface ICustomerProvider
    {
        Task<(bool IsSuccess, IEnumerable<Model.Customer> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, Model.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id);
    }
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<CustomerProvider> _logger;
        private readonly IMapper _mapper;

        public CustomerProvider(CustomerDbContext dbContext, ILogger<CustomerProvider> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Jessica Smith", Address = "20 Elm St." });
                _dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "John Smith", Address = "30 Main St." });
                _dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "William Johnson", Address = "100 10th St." });
                _dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Model.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                _logger?.LogInformation("Querying customers");
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    _logger?.LogInformation("Customer found");
                    var result = _mapper.Map<Db.Customer, Model.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Model.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                _logger?.LogInformation("Querying customers");
                var customers = await _dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    _logger?.LogInformation($"{customers.Count} customer(s) found");
                    var result = _mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Model.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
