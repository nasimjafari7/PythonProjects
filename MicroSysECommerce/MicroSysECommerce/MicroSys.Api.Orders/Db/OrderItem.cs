using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSys.Api.Orders.Db
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
