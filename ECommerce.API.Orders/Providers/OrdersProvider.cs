using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private IMapper mapper;
        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var order = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();

                if (order != null)
                {
                    var result = this.mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(order);

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

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Total = 175.00m,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 19, UnitPrice = 20m },
                        new Db.OrderItem { Id = 2, OrderId = 2, ProductId = 3, Quantity = 149, UnitPrice = 150m },
                        new Db.OrderItem { Id = 3, OrderId = 3, ProductId = 2, Quantity = 199, UnitPrice = 5m },
                    }
                }) ;

                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Total = 170.0m,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem { Id = 4, OrderId = 4, ProductId = 1, Quantity = 18, UnitPrice = 20m },
                        new Db.OrderItem { Id = 5, OrderId = 5, ProductId = 3, Quantity = 148, UnitPrice = 150m },
                    }
                });

                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 3,
                    OrderDate = DateTime.Now,
                    Total = 500.0m,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem { Id = 6, OrderId = 6, ProductId = 4, Quantity = 99, UnitPrice = 200m },
                        new Db.OrderItem { Id = 7, OrderId = 7, ProductId = 3, Quantity = 147, UnitPrice = 150m },
                        new Db.OrderItem { Id = 8, OrderId = 7, ProductId = 3, Quantity = 146, UnitPrice = 150m },
                    }
                });

                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 4,
                    CustomerId = 4,
                    OrderDate = DateTime.Now,
                    Total = 620.0m,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem { Id = 9, OrderId = 9, ProductId = 1, Quantity = 17, UnitPrice = 20m },
                        new Db.OrderItem { Id = 10, OrderId = 10, ProductId = 3, Quantity = 145, UnitPrice = 150m },
                        new Db.OrderItem { Id = 11, OrderId = 11, ProductId = 3, Quantity = 144, UnitPrice = 150m },
                        new Db.OrderItem { Id = 12, OrderId = 12, ProductId = 3, Quantity = 144, UnitPrice = 150m },
                        new Db.OrderItem { Id = 13, OrderId = 13, ProductId = 3, Quantity = 144, UnitPrice = 150m },
                    }
                });
                dbContext.SaveChanges();
            }
        }    
    }
}
