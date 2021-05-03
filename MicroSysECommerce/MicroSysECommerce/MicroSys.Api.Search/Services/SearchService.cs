using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSys.Api.Search.Services
{
    public interface ISearchService
    {
        Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId);
    }

    public class SearchService : ISearchService
    {
        private readonly IProductService _productsService;
        private readonly ICustomerService _customersService;
        private readonly IOrderService _ordersService;

        public SearchService(IProductService productsService, ICustomerService customersService, IOrderService ordersService)
        {
            _productsService = productsService;
            _customersService = customersService;
            _ordersService = ordersService;
        }

        /// <summary>
        /// Retrieves all orders of a specified customer including all order items with the relevant product name
        /// This method collects data from different services and mergers them together.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>
        /// A tuple including:
        /// IsSuccess: indicates if the requested data is collected successfully.
        /// SearchResults: Including the Customer information and the Orders
        /// </returns>
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customersResult = await _customersService.GetCustomerAsync(customerId);
            var ordersResult = await _ordersService.GetOrdersAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var orders in ordersResult.Orders)
                {
                    foreach (var item in orders.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product information is not available";
                    }
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ?
                                customersResult.Customer :
                                new { Name = "Customer information is not available" },
                    Orders = ordersResult.Orders
                };

                return (true, result);
            }
            return (false, null);
        }
    }
}
