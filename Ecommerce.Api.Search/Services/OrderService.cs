﻿using Ecommerce.Api.Search.Interfaces;
using Ecommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrderService> logger;
        public OrderService(IHttpClientFactory httpClientFactory, ILogger<OrderService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try {
                var client = httpClientFactory.CreateClient("OrderService");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            } catch(Exception e)
            {
                logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
