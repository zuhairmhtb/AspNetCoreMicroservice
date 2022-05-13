using AutoMapper;
using Ecommerce.Api.Orders.Db;
using Ecommerce.Api.Orders.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(
            OrdersDbContext dbContext,
            ILogger<OrdersProvider> logger,
            IMapper mapper
        )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        void SeedData() {
            //if (!dbContext.OrderItems.Any())
            //{
            //    dbContext.OrderItems.Add(new OrderItem() { Id = 1, ProductId = 1, Quantity = 4, UnitPrice = 20 });
            //    dbContext.OrderItems.Add(new OrderItem() { Id = 2, ProductId = 2, Quantity = 4, UnitPrice = 5 });
            //    dbContext.OrderItems.Add(new OrderItem() { Id = 3, ProductId = 3, Quantity = 1, UnitPrice = 150 });
            //    dbContext.OrderItems.Add(new OrderItem() { Id = 4, ProductId = 4, Quantity = 1, UnitPrice = 200 });
            //    dbContext.SaveChanges();
            //}

            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow,
                    Total = 100,
                    Items = new List<OrderItem>() {
                        new OrderItem(){ Id = 1, ProductId = 1, Quantity = 4, UnitPrice = 20, OrderId = 1 },
                        new OrderItem(){ Id = 2, ProductId = 2, Quantity = 4, UnitPrice = 5, OrderId = 1 }
                    }
                });
                dbContext.Orders.Add(new Order()
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.UtcNow,
                    Total = 350,
                    Items = new List<OrderItem>() {
                        new OrderItem(){ Id = 3, ProductId = 3, Quantity = 1, UnitPrice = 150, OrderId = 2 },
                        new OrderItem(){ Id = 4, ProductId = 4, Quantity = 1, UnitPrice = 200, OrderId = 2 }
                    }
                });
                dbContext.SaveChanges();
            }
            
        }

        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id)
        {
            try
            {
                var order = await dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(x => x.Id == id);
                if (order != null)
                {
                    var result = mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.Include(o => o.Items).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetCustomerOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(x => x.CustomerId == customerId).Include(o => o.Items).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
