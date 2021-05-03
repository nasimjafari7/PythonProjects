using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSys.Api.Products.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
    }
}
