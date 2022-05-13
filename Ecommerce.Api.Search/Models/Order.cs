using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
