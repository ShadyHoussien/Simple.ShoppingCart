using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.ShoppingCart.Models.PaymentModels.StepThree
{
    public class PaymentKeyRequest
    {
        public string auth_token { get; set; }
        public decimal amount_cents { get; set; }
        public int expiration { get; set; }
        public int order_id { get; set; }
        public BillingData billing_data { get; set; }
        public string currency { get; set; }
        public int integration_id { get; set; }
    }
}
