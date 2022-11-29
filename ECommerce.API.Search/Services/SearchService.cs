using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {

            var customersResult = await customersService.GetCustomersAsync(customerId);
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            
            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product information is not available";//This is looking for the product name via its ID
                    }
                }

                var result = new
                {
                    Customer = customersResult.IsSuccess ?
                    customersResult.Customer : new Models.Customer { Name = "Customer information is not available", Address="N/A" }, //Uses the second readymade instantiation in case customers api doesn't work
                    Orders = ordersResult.Orders
                };

               

                return (true, result);
            }

            //This is separate because I couldn't get it to object instantiate in the previous 'if' statement. It essentially
            //populates the IEnum customer models object. Should in the event the customer API not be available
            //then the customer's name will hold "Customer information is not available", but the Orders and Products
            //objects should be still available for the user to reach

           

            return (false, null);
        }
    }
}
