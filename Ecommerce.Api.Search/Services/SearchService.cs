using Ecommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;
        public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            var productResult = await productService.GetProductsAsync();
            if(orderResult.IsSuccess)
            {
                foreach(var order in orderResult.Orders)
                {
                    var customerResult = await customerService.GetCustomerAsync(order.CustomerId);
                    if(customerResult.IsSuccess)
                    {
                        order.CustomerName = customerResult.Customer.Name;
                    }
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productResult.Products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
                    }
                }
                var result = new
                {
                    Orders = orderResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
