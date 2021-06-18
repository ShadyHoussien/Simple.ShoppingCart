using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.ShoppingCart.Models.PaymentModels.StepTwo
{
    public class OrderRegistrationRequest
    {
        public string auth_token { get; set; }
        public bool delivery_needed { get; set; }
        public decimal amount_cents { get; set; }
        public List<OrderItem> items { get; set; }
    }
}
