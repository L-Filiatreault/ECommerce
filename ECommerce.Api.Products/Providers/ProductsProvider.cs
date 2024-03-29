﻿using AutoMapper;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();

        }

        private void SeedData()
        {
            if(!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id=1, Name="Keyboard", Price=20, Inventory=100});
                dbContext.Products.Add(new Db.Product() { Id=2, Name="Mouse", Price=5, Inventory=200});
                dbContext.Products.Add(new Db.Product() { Id=3, Name="Monitor", Price=150, Inventory=100});
                dbContext.Products.Add(new Db.Product() { Id=4, Name="CPU", Price=200, Inventory=100});
                dbContext.SaveChanges();
            }

            
        }
        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    var result = this.mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
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

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = this.mapper.Map<Db.Product, Models.Product>(product);
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
    }
}
