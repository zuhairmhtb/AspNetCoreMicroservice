using Ecommerce.Api.Orders.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;
        public OrderController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync() {
            var result = await ordersProvider.GetOrdersAsync();
            if (result.IsSuccess) return Ok(result.Orders);
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var result = await ordersProvider.GetCustomerOrdersAsync(id);
            if (result.IsSuccess) return Ok(result.Orders);
            return NotFound();
        }
    }
}
