using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.ShoppingCart.Models.PaymentModels.StepTwo
{
    public class OrderItem
    {
        public string name { get; set; }
        public string amount_cents { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
    }
}
