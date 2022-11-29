using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<IProductsService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<IProductsService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }



        public async Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/customers/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Customer>(content, options);
                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
