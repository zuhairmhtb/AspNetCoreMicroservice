using Ecommerce.Api.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Interface
{
    public interface IOrdersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id);
        Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetCustomerOrdersAsync(int customerId);
    }
}
