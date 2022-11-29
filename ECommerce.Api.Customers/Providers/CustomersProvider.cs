using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private IMapper mapper;
        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }


        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Bruce Wayne", Address = "88 Gotham Boulevard, Montreal, QC, Canada, X3W 9W9" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Clark Kent", Address = "101 Metropolis Lane, Montreal, QC, Canada, T9T 1X1" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Thomas Shelby", Address = "62 Evergreen street, Montreal, QC, Canada, W3W 4X4" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Tony Stark", Address = "60 Finch Avenue, Montreal, QC, Canada, M2N 3A1" });
                dbContext.SaveChanges();
            }


        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);

                }
                return (false, null, "Not Found");
            }
            catch (Exception error)
            {
                logger?.LogError(error.ToString());
                return (false, null, error.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer , string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if(customer !=null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

       
    }
}
