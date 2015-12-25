using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exemplo3.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public string userName { get; set; }

        public decimal precoFrete { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}